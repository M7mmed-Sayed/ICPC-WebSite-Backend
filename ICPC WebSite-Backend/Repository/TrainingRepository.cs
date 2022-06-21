using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Utility;

namespace ICPC_WebSite_Backend.Repository
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TrainingRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response> AddTraining(TrainingDTO trainingDTO) {
            var ret = new Response();

            var training = new Training() {
                Title = trainingDTO.Title,
                Level = trainingDTO.Level,
                IsPublic = trainingDTO.IsPublic,
                Community_Id = trainingDTO.Community_Id,
                Created_At = DateTime.Now
            };
            await _applicationDbContext.trainings.AddAsync(training);
            await _applicationDbContext.SaveChangesAsync();
            return ret;
        }
        public async Task<Response> UpdateTraining(int trainingId, TrainingDTO trainingDTO) {
            var ret = new Response();
            var training = await _applicationDbContext.trainings.FindAsync(trainingId);

            if (training != null) {
                training.Title = trainingDTO.Title;
                training.Level = trainingDTO.Level;
                training.IsPublic = trainingDTO.IsPublic;
                training.Community_Id = trainingDTO.Community_Id;
                training.Updated_At = DateTime.Now;
                await _applicationDbContext.SaveChangesAsync();
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.TrainingNotFound);
            }
            return ret;
        }
        public async Task<Response> GetAllTrainings() {
            var ret = new Response();
            var trainings = _applicationDbContext.trainings.ToList();
            ret.Data = trainings;
            return ret;
        }
        public async Task<Response> GetTraining(int trainingId) {
            var ret = new Response();
            var training = await _applicationDbContext.trainings.FindAsync(trainingId);
            if (training == null) {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.TrainingNotFound);
            }
            else {
                ret.Data = training;
            }
            return ret;
        }
        public async Task<Response> DeleteTraining(int trainingId) {
            var ret = new Response();
            var training = await _applicationDbContext.trainings.FindAsync(trainingId);
            if (training != null) {
                _applicationDbContext.trainings.Remove(training);
                await _applicationDbContext.SaveChangesAsync();
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.TrainingNotFound);
            }
            return ret;

        }
    }
}
