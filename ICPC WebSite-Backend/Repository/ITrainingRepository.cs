using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ITrainingRepository
    {
        /// <summary>
        /// add a training to a community
        /// </summary>
        /// <param name="trainingDTO">the data of the training</param>
        /// <returns>training if added succeeded otherwise error</returns>
        Task<Response<Training>> AddTraining(TrainingDTO trainingDTO);
        /// <summary>
        /// delete a specific training
        /// </summary>
        /// <param name="trainingId">the id of the training</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> DeleteTraining(int trainingId);
        /// <summary>
        /// get all trainings in a community
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <returns>list of all trainings</returns>
        Task<Response<IEnumerable<TrainingResponse>>> GetAllTrainings(int communityId);
        /// <summary>
        /// get a specific training
        /// </summary>
        /// <param name="trainingId">the id of the training</param>
        /// <returns>training data if succeeded if exist otherwise an error returned</returns>
        Task<Response<Training>> GetTraining(int trainingId);
        /// <summary>
        /// update training data with new data
        /// </summary>
        /// <param name="trainingId">id of the training</param>
        /// <param name="trainingDTO">the new data of training</param>
        /// <returns>if succeeded return the updated training otherwise returns an error</returns>
        Task<Response<Training>> UpdateTraining(int trainingId, TrainingDTO trainingDTO);
        /// <summary>
        /// link a week to an existing training
        /// </summary>
        /// <param name="trainingId">the id of the training</param>
        /// <param name="weekId">the id of the week</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> LinkWeek(int trainingId, int weekId);
        /// <summary>
        /// unlink a week from a training
        /// </summary>
        /// <param name="trainingId">the id of the training</param>
        /// <param name="weekId">the id of the week</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> UnLinkWeek(int trainingId, int weekId);
        /// <summary>
        /// join user to a training
        /// </summary>
        /// <param name="userId">the id of the user</param>
        /// <param name="trainingId">the id of the training</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> JoinTraining(string userId, int trainingId);
        /// <summary>
        /// get all users in this training with specific status
        /// </summary>
        /// <param name="trainingId">the id of the training</param>
        /// <param name="status"></param>
        /// <returns>list of members</returns>
        Task<Response<IEnumerable<TrainingMemberDto>>> GetTrainingMembersAsync(int trainingId,string status);
        /// <summary>
        /// accept or reject a training request
        /// </summary>
        /// <param name="userId">the id of the user</param>
        /// <param name="trainingId">the id of training</param>
        /// <param name="accept">if true then accept request otherwise reject it</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> ResponseToTrainingRequest(string userId, int trainingId, bool accept);
    }
}
