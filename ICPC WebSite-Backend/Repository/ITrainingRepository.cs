using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ITrainingRepository
    {
        Task<Response> AddTraining(TrainingDTO trainingDTO);
        Task<Response> DeleteTraining(int trainingId);
        Task<Response<IEnumerable<Training>>> GetAllTrainings();
        Task<Response<Training>> GetTraining(int trainingId);
        Task<Response> UpdateTraining(int trainingId, TrainingDTO trainingDTO);
        Task<Response> LinkWeek(int trainingId, int weekId);
    }
}
