using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ITrainingRepository
    {
        Task<Response<Training>> AddTraining(TrainingDTO trainingDTO);
        Task<Response> DeleteTraining(int trainingId);
        Task<Response<IEnumerable<TrainingResponse>>> GetAllTrainings(int communityId);
        Task<Response<Training>> GetTraining(int trainingId);
        Task<Response<Training>> UpdateTraining(int trainingId, TrainingDTO trainingDTO);
        Task<Response> LinkWeek(int trainingId, int weekId);
        Task<Response> UnLinkWeek(int trainingId, int weekId);
        Task<Response> JoinTraining(string userId, int trainingId);
        Task<Response<IEnumerable<TrainingMemberDto>>> GetTrainingMembersAsync(int trainingId,string status);
        Task<Response> ResponseToTrainingRequest(string userId, int trainingId, bool accept);
    }
}
