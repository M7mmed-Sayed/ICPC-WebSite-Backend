using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public CommunityRepository(ApplicationDbContext applicationDbContext,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender) {
            _userManager = userManager;
            _emailSender = emailSender;
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public async Task<Response> GetAllCommunities() {
            var ret = new Response();
            ret.Data = new List<Community>();
            ((List<Community>)ret.Data).AddRange(_applicationDbContext.communities.ToList());
            return ret;
        }
        public async Task<Response> GetCommunity(int id) {
            var ret = new Response();
            ret.Data = await _applicationDbContext.communities.FindAsync(id);
            return ret;
        }

        public async Task<Response> RegisterCommunityAsync(CommunityDTO communityDTO) {
            var ret = new Response();
            try {
                var community = new Community() {
                    Name = communityDTO.Name,
                    About = communityDTO.About,
                    OfficialMail = communityDTO.OfficialMail,
                    RequesterId = communityDTO.RequesterId
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
        public async Task<Response> AcceptCommunity(int communityId) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.FindAsync(communityId);

                if (community != null) {
                    var user = await _userManager.FindByIdAsync(community.RequesterId);
                    if (user == null) {
                        ret.Succeeded = false;
                        ret.Errors.Add(ErrorsList.CannotFindUser);
                        return ret;
                    }
                    await _userManager.AddToRoleAsync(user, RolesList.CommunityLeader);
                    community.IsApproved = true;
                    await _applicationDbContext.SaveChangesAsync();

                    //Accepted Mail
                    var message = $"congratulations {user.FirstName} <br>";
                    message += $"Your request for Creating {community.Name} Community was Accepted" +
                        $" <br> Welcome again for you and for {community.Name} community mempers's  <br>" +
                        $"We assigned you to be {community.Name} Leader "
                        ;
                    var subject = "Competitve Programing Registeration Community";
                    var emailSendResult = _emailSender.SendEmail(user.Email, subject, message);
                    if (emailSendResult.Succeeded == false) {
                        ret.Succeeded = false;
                        ret.Errors.AddRange(emailSendResult.Errors);
                    }
                }
                else {
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                }
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;
                ret.Errors.Add(err);
            }
            return ret;
        }
        public async Task<Response> RejectCommunity(int communityId) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.FindAsync(communityId);

                if (community != null) {
                    var user = await _userManager.FindByIdAsync(community.RequesterId);
                    if (user == null) {
                        ret.Succeeded = false;
                        ret.Errors.Add(ErrorsList.CannotFindUser);
                        return ret;
                    }
                    //Reject Mail
                    var message = $"Dear  {user.FirstName} <br>";
                    message += $"I’d like to thank about your request to create {community.Name}" +
                        $" <br>Unfortunately, after careful consideration We decided to Rejec your request try again after 2 Weeks <br>" +
                        $"Again, thank you for your interest in creating a Community " +
                        $"for more inf send XYZ@gmail.com"
                        ;
                    var subject = "Competitve Programing Registeration Community";
                    _applicationDbContext.communities.Remove(community);
                    await _applicationDbContext.SaveChangesAsync();
                    var emailSendResult = _emailSender.SendEmail(user.Email, subject, message);
                    if (emailSendResult.Succeeded == false) {
                        ret.Succeeded = false;
                        ret.Errors.AddRange(emailSendResult.Errors);
                    }
                }
                else {
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                }
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;
                ret.Errors.Add(err);
            }
            return ret;
        }

        public async Task<Response> AssignRole(string userId, int communityId, string roleName) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.FindAsync(communityId);
                if (community == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CannotFindUser);
                }
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.InvalidRoleName);
                }
                if (!ret.Succeeded)
                    return ret;
                var request = await _applicationDbContext.CommunityRequests.Where(x => x.MemberId == userId && x.CommunityId == communityId && x.Status == ConstVariable.AcceptedStatus).SingleOrDefaultAsync();
                if (request == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.RequestNotAcceptOrNotFound);
                    return ret;
                }
                if (!await _userManager.IsInRoleAsync(user, roleName)) {
                    var res = await _userManager.AddToRoleAsync(user, roleName);
                    if (!res.Succeeded) {
                        ret.Succeeded = false;
                        ret.Errors.AddRange(res.Errors.Select(x => new Error {
                            Code = x.Code,
                            Description = x.Description
                        }));
                        return ret;
                    }
                }
                await _applicationDbContext.CommunityMember.AddAsync(new CommunityMember {
                    MemberId = userId,
                    CommunityId = communityId,
                    Role = roleName
                });
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
        public async Task<Response> GetMembers(int communityId) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.Include(x => x.CommunityMembers).ThenInclude(m => m.Member).Where(x => x.Id == communityId).SingleOrDefaultAsync();
                if (community == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                    return ret;
                }
                ret.Data = community.CommunityMembers.GroupBy(x => new { x.MemberId, x.Member.FirstName, x.Member.LastName, x.Member.Email, x.Member.UserName }, (key, val) => new {
                    Id = key.MemberId,
                    FirstName = key.FirstName,
                    LastName = key.LastName,
                    Email = key.Email,
                    UserName = key.UserName,
                    Roles = val.Select(x => x.Role).ToList()
                });
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;
                ret.Errors.Add(err);
            }
            return ret;
        }
        public async Task<Response> CountMembers(int communityId) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.Include(x => x.CommunityMembers).ThenInclude(m => m.Member).Where(x => x.Id == communityId).SingleOrDefaultAsync();
                if (community == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                    return ret;
                }
                ret.Data = community.CommunityMembers.GroupBy(x => x.MemberId).Count();
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;
                ret.Errors.Add(err);
            }
            return ret;
        }

        public async Task<Response> JoinRequest(string userId, int communityId) {
            var ret = new Response();
            try {
                var community = await _applicationDbContext.communities.FindAsync(communityId);
                if (community == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CommunityNotFound);
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.CannotFindUser);
                }
                if (!ret.Succeeded)
                    return ret;

                await _applicationDbContext.CommunityRequests.AddAsync(new CommunityRequest {
                    MemberId = userId,
                    CommunityId = communityId,
                    Status = ConstVariable.PendingStatus
                });
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
        public async Task<Response> GetRequest(int communityId) {
            var ret = new Response();
            try {
                ret.Data = await _applicationDbContext.CommunityRequests.Include(x => x.Member).Where(x => x.CommunityId == communityId && x.Status == ConstVariable.PendingStatus).Select(x=>new {
                    x.MemberId,
                    x.Member.FirstName,
                    x.Member.LastName,
                    x.Member.Email,
                    x.Member.UserName
                }).ToListAsync();
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;
                ret.Errors.Add(err);
            }
            return ret;
        }
        public async Task<Response> ResponseToRequest(string userId, int communityId, bool accept) {
            var ret = new Response();
            try {
                var request = await _applicationDbContext.CommunityRequests.Where(x => x.MemberId == userId && x.CommunityId == communityId && x.Status == ConstVariable.PendingStatus).SingleOrDefaultAsync();
                if (request == null) {
                    ret.Succeeded = false;
                    ret.Errors.Add(ErrorsList.JoinRequestNotFound);
                    return ret;
                }
                if (accept) {
                    request.Status = ConstVariable.AcceptedStatus;
                    ret.Data = "User has been approved successfully";
                }
                else {
                    request.Status = ConstVariable.RejectedStatus;
                    ret.Data = "User has been Rejected successfully";
                }
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
