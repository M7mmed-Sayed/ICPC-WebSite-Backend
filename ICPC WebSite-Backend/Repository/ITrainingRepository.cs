using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ITrainingRepository
    {
        Task<Response> AddTraining(TrainingDTO trainingDTO);
        Task<Response> DeleteTraining(int trainingId);
        Task<Response> GetAllTrainings();
        Task<Response> GetTraining(int trainingId);
        Task<Response> UpdateTraining(int trainingId, TrainingDTO trainingDTO);
    }
}
