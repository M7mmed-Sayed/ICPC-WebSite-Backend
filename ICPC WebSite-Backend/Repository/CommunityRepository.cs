using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;

namespace ICPC_WebSite_Backend.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        public CommunityRepository(ApplicationDbContext applicationDbContext,
            UserManager<User> userManager, IEmailSender emailSender) {
            _userManager = userManager;
            _emailSender = emailSender;
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
        public async Task<ValidateResponse> AcceptCommunity(int communityId) {
            var ret = new ValidateResponse();
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
            return ret;
        }
        public async Task<ValidateResponse> RejectCommunity(int communityId) {
            var ret = new ValidateResponse();
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
            return ret;
        }


    }
}
