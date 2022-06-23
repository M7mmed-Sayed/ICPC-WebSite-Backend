﻿using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;

namespace ICPC_WebSite_Backend.Repository
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TrainingRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response> AddTraining(TrainingDTO trainingDTO)
        {
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
            return ResponseFactory.Ok();
        }

        public async Task<Response> UpdateTraining(int trainingId, TrainingDTO trainingDTO)
        {
            var training = await _applicationDbContext.Trainings.FindAsync(trainingId);

            if (training == null) return ResponseFactory.Fail(ErrorsList.TrainingNotFound);
            training.Title = trainingDTO.Title;
            training.Level = trainingDTO.Level;
            training.IsPublic = trainingDTO.IsPublic;
            training.CommunityId = trainingDTO.Community_Id;
            training.UpdatedAt = DateTime.Now;
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }

        public async Task<Response<IEnumerable<Training>>> GetAllTrainings()
        {
            var trainings = _applicationDbContext.Trainings.ToList();
            return ResponseFactory.Ok<IEnumerable<Training>>(trainings);
        }

        public async Task<Response<Training>> GetTraining(int trainingId)
        {
            var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
            if (training == null)
                return ResponseFactory.Fail<Training>(ErrorsList.TrainingNotFound);

            return ResponseFactory.Ok(training);
        }

        public async Task<Response> DeleteTraining(int trainingId)
        {
            var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
            if (training == null)
                return ResponseFactory.Fail<Training>(ErrorsList.TrainingNotFound);
            _applicationDbContext.Trainings.Remove(training);
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
    }
}