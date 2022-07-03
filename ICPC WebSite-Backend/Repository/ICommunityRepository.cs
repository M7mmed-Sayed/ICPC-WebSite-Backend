using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface ICommunityRepository
{
    /// <summary>
    /// register a community in the website
    /// </summary>
    /// <param name="communityDto">community data</param>
    /// <returns>returns the created community</returns>
    Task<Response<Community>> RegisterCommunityAsync(CommunityDto communityDto);
    /// <summary>
    /// get all communities exist in the website
    /// </summary>
    /// <returns>return list of communities data</returns>
    Task<Response<IEnumerable<Community>>> GetAllCommunities();
    /// <summary>
    /// get a community
    /// </summary>
    /// <param name="id">the id of the community</param>
    /// <returns>returns the community data if exist otherwise returns an error</returns>
    Task<Response<Community>> GetCommunity(int id);
    /// <summary>
    /// assign a role for a specific user in the community with communityId
    /// </summary>
    /// <param name="userId">the id of the uesr</param>
    /// <param name="communityId">the id of the community</param>
    /// <param name="roleName">the name of the rule</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> AssignRole(string userId, int communityId, string roleName);
    /// <summary>
    /// get all members of a specific community
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>returns list of all members in the community</returns>
    Task<Response<IEnumerable<CommunityMemberDto>>> GetMembers(int communityId);
    /// <summary>
    /// count the number of members in the community
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>return an integer represents the number of members</returns>
    Task<Response<int>> CountMembers(int communityId);
    /// <summary>
    /// make a request from specific user to join specific community
    /// </summary>
    /// <param name="userId">the id of the user</param>
    /// <param name="communityId">the id of the community</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> JoinRequest(string userId, int communityId);
    /// <summary>
    /// accept of reject a joining request for a community
    /// </summary>
    /// <param name="userId">the id of the user who made a request</param>
    /// <param name="communityId">the community which the user want to join</param>
    /// <param name="accept">bool variable to determine accept the request or not</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> ResponseToRequest(string userId, int communityId, bool accept);
    /// <summary>
    /// get all members that make a request for a specific member
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>return list of community members</returns>
    Task<Response<IEnumerable<CommunityMemberDto>>> GetRequest(int communityId);
    /// <summary>
    /// edit community data
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <param name="communityDto">current data of the community</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> EditCommunity(int communityId, CommunityDto communityDto);
    /// <summary>
    /// delete a specific communicty
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> DeleteCommunity(int communityId);
    Task<Response> RemoveRole(string userId, int communityId, string roleName);
    Task<Response> KickUser(string userId);
}