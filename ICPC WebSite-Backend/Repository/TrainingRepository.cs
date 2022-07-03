using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Repository
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;

        public TrainingRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Training>> AddTraining(TrainingDTO trainingDTO)
        {
            try
            {
                var community = await _applicationDbContext.Communities.FindAsync(trainingDTO.Community_Id);
                if (community == null) return ResponseFactory.Fail<Training>(ErrorsList.CommunityNotFound);
                var training = new Training()
                {
                    Title = trainingDTO.Title,
                    Level = trainingDTO.Level,
                    IsPublic = trainingDTO.IsPublic,
                    CommunityId = trainingDTO.Community_Id,
                    CreatedAt = DateTime.Now
                };
                await _applicationDbContext.Trainings.AddAsync(training);
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok<Training>(training);
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException<Training>(ex);
            }
        }

        public async Task<Response<Training>> UpdateTraining(int trainingId, TrainingDTO trainingDTO)
        {
            try
            {
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);

                if (training == null) return ResponseFactory.Fail<Training>(ErrorsList.TrainingNotFound);
                training.Title = trainingDTO.Title;
                training.Level = trainingDTO.Level;
                training.IsPublic = trainingDTO.IsPublic;
                training.CommunityId = trainingDTO.Community_Id;
                training.UpdatedAt = DateTime.Now;
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok(training);
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException<Training>(ex);
            }
        }

        public async Task<Response<IEnumerable<TrainingResponse>>> GetAllTrainings(int communityId)
        {
            try
            {
                var community = await _applicationDbContext.Communities.Where(community=>community.Id==communityId).Include(x=>x.Trainings).FirstOrDefaultAsync();
                if (community == null)
                    return ResponseFactory.Fail<IEnumerable<TrainingResponse>>(ErrorsList.CommunityNotFound);
                var data = community.Trainings.Select(training =>new TrainingResponse()
                {
                    Id=training.Id,
                    Title = training.Title,
                    Level = training.Level
                } ).ToList();
                return ResponseFactory.Ok<IEnumerable<TrainingResponse>>(data);
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException<IEnumerable<TrainingResponse>>(ex);
            }
        }

        public async Task<Response<Training>> GetTraining(int trainingId)
        {
            try
            {
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
                return training == null
                    ? ResponseFactory.Fail<Training>(ErrorsList.TrainingNotFound)
                    : ResponseFactory.Ok(training);
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException<Training>(ex);
            }
        }

        public async Task<Response> DeleteTraining(int trainingId)
        {
            try
            {
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
                if (training == null)
                    return ResponseFactory.Fail<Training>(ErrorsList.TrainingNotFound);
                _applicationDbContext.Trainings.Remove(training);
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException(ex);
            }
        }

        public async Task<Response> LinkWeek(int trainingId, int weekId)
        {
            try
            {
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
                var errors = new List<Error>();
                if (training == null)
                    errors.Add(ErrorsList.TrainingNotFound);
                var week = await _applicationDbContext.Weeks.FindAsync(weekId);
                if (week == null)
                    errors.Add(ErrorsList.WeekNotFound);
                if (errors.Count != 0)
                    return ResponseFactory.Fail(errors);
                var weekTraining =
                    _applicationDbContext.WeeksTrainings.FirstOrDefault(w =>
                        w.TrainingId == trainingId && w.WeekId == weekId);
                if (weekTraining != null)
                    return ResponseFactory.Fail(ErrorsList.DublicateWeekAtTraining);
                weekTraining = new WeekTraining { WeekId = weekId, TrainingId = trainingId };
                await _applicationDbContext.WeeksTrainings.AddAsync(weekTraining);
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException(ex);
            }
        }

        public async Task<Response> UnLinkWeek(int trainingId, int weekId)
        {
            try
            {
                var weekTraining =
                    _applicationDbContext.WeeksTrainings.FirstOrDefault(w =>
                        w.TrainingId == trainingId && w.WeekId == weekId);
                if (weekTraining == null)
                    return ResponseFactory.Fail(ErrorsList.WeekNotAtTraining);
                _applicationDbContext.WeeksTrainings.Remove(weekTraining);
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException(ex);
            }
        }

        public async Task<Response> JoinTraining(string userId, int trainingId)
        {
            try
            {
                var errorsList = new List<Error>();
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
                if (training == null) errorsList.Add(ErrorsList.TrainingNotFound);
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) errorsList.Add(ErrorsList.CannotFindUser);
                var previousJoinRequest = await _applicationDbContext.TrainingRequests
                    .Where(request => request.MemberId == userId && request.TrainingId == trainingId)
                    .FirstOrDefaultAsync();
                if (previousJoinRequest != null) errorsList.Add(ErrorsList.ThereIsAPreviousRequest);
                if (errorsList.Any())
                    return ResponseFactory.Fail(errorsList);
                var result = await _applicationDbContext.TrainingRequests.AddAsync(new TrainingRequest
                {
                    MemberId = userId,
                    TrainingId = trainingId,
                    Status = ConstVariable.PendingStatus
                });
                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException(ex);
            }
        }

        public async Task<Response<IEnumerable<TrainingMemberDto>>> GetTrainingMembersAsync(int trainingId,
            string status)
        {
            try
            {
                var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
                if (training == null)
                    return ResponseFactory.Fail<IEnumerable<TrainingMemberDto>>(ErrorsList.TrainingNotFound);

                var data = await _applicationDbContext.TrainingRequests.Include(x => x.Member)
                    .Where(x => x.TrainingId == trainingId && x.Status == status)
                    .Select(x => new TrainingMemberDto()
                    {
                        Id = x.MemberId,
                        FirstName = x.Member.FirstName,
                        LastName = x.Member.LastName,
                    }).ToListAsync();
                return ResponseFactory.Ok<IEnumerable<TrainingMemberDto>>(data);
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException<IEnumerable<TrainingMemberDto>>(ex);
            }
        }

        public async Task<Response> ResponseToTrainingRequest(string userId, int trainingId, bool accept)
        {
            try
            {
                var request = await _applicationDbContext.TrainingRequests.Where(x =>
                        x.MemberId == userId && x.TrainingId == trainingId && x.Status == ConstVariable.PendingStatus)
                    .SingleOrDefaultAsync();

                if (request == null) return ResponseFactory.Fail(ErrorsList.JoinRequestNotFound);
                request.Status = accept ? ConstVariable.AcceptedStatus : ConstVariable.RejectedStatus;

                await _applicationDbContext.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            catch (Exception ex)
            {
                return ResponseFactory.FailFromException(ex);
            }
        }
    }
}