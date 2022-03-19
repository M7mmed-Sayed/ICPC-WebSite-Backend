﻿using ICPC_WebSite_Backend.Data;
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

        public async Task<ValidateResponse> GetAllCommunities() {
            var ret = new ValidateResponse();
            ret.Data = new List<Community>();
            ((List<Community>)ret.Data).AddRange(_applicationDbContext.communities.ToList());
            return ret;
        }

        public async Task<ValidateResponse> RegisterCommunityAsync(CommunityDTO communityDTO) {
            var ret = new ValidateResponse();
            try {
                var community = new Community() {
                    Name = communityDTO.Name,
                    About = communityDTO.About,
                    OfficialMail = communityDTO.OfficialMail
                };
                await _applicationDbContext.communities.AddAsync(community);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                ret.Errors.Add(new Error() { Code = ex.Message, Description = ex.InnerException.Message });
            }
            return ret;
        }
    }
}
