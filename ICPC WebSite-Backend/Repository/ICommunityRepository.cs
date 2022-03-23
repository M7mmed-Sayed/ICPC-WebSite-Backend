using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICommunityRepository
    {
        Task<ValidateResponse> RegisterCommunityAsync(CommunityDTO communityDTO);
        Task<ValidateResponse> GetAllCommunities();
    }
}