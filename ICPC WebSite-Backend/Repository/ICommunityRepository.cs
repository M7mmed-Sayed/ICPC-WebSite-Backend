using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICommunityRepository
    {
        Task<Response> RegisterCommunityAsync(CommunityDTO communityDTO);
        Task<Response> GetAllCommunities();
         Task<Response> AcceptCommunity(int communityId);
         Task<Response> RejectCommunity(int communityId);
    }
}