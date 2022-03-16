using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CommunityRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<ValidateResponse> RegisterCommunityAsync(CommunityDTO communityDTO) {
            var ret = new ValidateResponse();
            try {
                var community = new Community() {
                    Name = communityDTO.Name,
                    About = communityDTO.About,
                };
                await _applicationDbContext.communities.AddAsync(community);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;

                ret.Errors.Add(err);
            }
            return ret;
        }
    }
}
