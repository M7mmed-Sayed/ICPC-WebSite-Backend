using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface ICommunityRepository
{
    Task<Response> RegisterCommunityAsync(CommunityDTO communityDTO);
    Task<Response<IEnumerable<Community>>> GetAllCommunities();
    Task<Response<Community>> GetCommunity(int id);
    Task<Response> AcceptCommunity(int communityId);
    Task<Response> RejectCommunity(int communityId);
    Task<Response> AssignRole(string userId, int communityId, string roleName);
    Task<Response<IEnumerable<CommunityMemberDTO>>> GetMembers(int communityId);
    Task<Response<int>> CountMembers(int communityId);
    Task<Response> JoinRequest(string userId, int communityId);
    Task<Response> ResponseToRequest(string userId, int communityId, bool accept);
    Task<Response<IEnumerable<CommunityMemberDTO>>> GetRequest(int communityId);
}