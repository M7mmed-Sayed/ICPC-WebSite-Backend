using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;

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