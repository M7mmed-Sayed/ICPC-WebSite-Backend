using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Repository;

public class CommunityRepository : ICommunityRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public CommunityRepository(
        ApplicationDbContext applicationDbContext,
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _applicationDbContext = applicationDbContext;
        _roleManager = roleManager;
    }

    public async Task<Response<IEnumerable<Community>>> GetAllCommunities() =>
        ResponseFactory.Ok<IEnumerable<Community>>(await _applicationDbContext.communities.ToListAsync());

    public async Task<Response<Community>> GetCommunity(int id)
    {
        var data = await _applicationDbContext.communities.FindAsync(id);
        return data is null
            ? ResponseFactory.Fail<Community>(ErrorsList.CommunityNotFound)
            : ResponseFactory.Ok(data);
    }

    public async Task<Response> RegisterCommunityAsync(CommunityDTO communityDTO)
    {
        try
        {
            var community = new Community
            {
                Name = communityDTO.Name,
                About = communityDTO.About,
                OfficialMail = communityDTO.OfficialMail,
                RequesterId = communityDTO.RequesterId
            };
            await _applicationDbContext.communities.AddAsync(community);
            var result = await _applicationDbContext.SaveChangesAsync();
            return result != 0 ? ResponseFactory.Ok() : ResponseFactory.Fail();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> AcceptCommunity(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.communities.FindAsync(communityId);

            if (community == null) return ResponseFactory.Fail(ErrorsList.CommunityNotFound);

            var user = await _userManager.FindByIdAsync(community.RequesterId);

            if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);

            var result = await _userManager.AddToRoleAsync(user, RolesList.CommunityLeader);

            if (result.Succeeded == false) return result.ToApplicationResponse();

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

            return emailSendResult;
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> RejectCommunity(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.communities.FindAsync(communityId);

            if (community == null) return ResponseFactory.Fail(ErrorsList.CommunityNotFound);

            var user = await _userManager.FindByIdAsync(community.RequesterId);

            if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);

            //Reject Mail
            var message = $"Dear  {user.FirstName} <br>";
            message += $"I’d like to thank about your request to create {community.Name}" +
                       " <br>Unfortunately, after careful consideration We decided to Rejec your request try again after 2 Weeks <br>" +
                       "Again, thank you for your interest in creating a Community " +
                       "for more inf send XYZ@gmail.com"
                ;
            var subject = "Competitve Programing Registeration Community";
            _applicationDbContext.communities.Remove(community);
            await _applicationDbContext.SaveChangesAsync();
            var emailSendResult = _emailSender.SendEmail(user.Email, subject, message);

            return emailSendResult;
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> AssignRole(string userId, int communityId, string roleName)
    {
        try
        {
            var errorsList = new List<Error>();
            var community = await _applicationDbContext.communities.FindAsync(communityId);

            if (community == null) errorsList.Add(ErrorsList.CommunityNotFound);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) errorsList.Add(ErrorsList.CannotFindUser);

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist) errorsList.Add(ErrorsList.InvalidRoleName);

            if (errorsList.Any()) return ResponseFactory.Fail(errorsList);

            var request = await _applicationDbContext.CommunityRequests.Where(x =>
                    x.MemberId == userId && x.CommunityId == communityId && x.Status == ConstVariable.AcceptedStatus)
                .SingleOrDefaultAsync();

            if (request == null) return ResponseFactory.Fail(ErrorsList.RequestNotAcceptOrNotFound);

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var res = await _userManager.AddToRoleAsync(user, roleName);

                if (!res.Succeeded) return res.ToApplicationResponse();
            }

            await _applicationDbContext.CommunityMember.AddAsync(new CommunityMember
            {
                MemberId = userId,
                CommunityId = communityId,
                Role = roleName
            });
            await _applicationDbContext.SaveChangesAsync();

            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response<IEnumerable<CommunityMemberDTO>>> GetMembers(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.communities.Include(x => x.CommunityMembers)
                .ThenInclude(m => m.Member).Where(x => x.Id == communityId).SingleOrDefaultAsync();

            if (community == null)
                return ResponseFactory.Fail<IEnumerable<CommunityMemberDTO>>(ErrorsList.CommunityNotFound);

            var data = community.CommunityMembers.GroupBy(
                x => new {x.MemberId, x.Member.FirstName, x.Member.LastName, x.Member.Email, x.Member.UserName},
                (key, val) => new CommunityMemberDTO
                {
                    Id = key.MemberId,
                    FirstName = key.FirstName,
                    LastName = key.LastName,
                    Email = key.Email,
                    UserName = key.UserName,
                    Roles = val.Select(x => x.Role).ToList()
                });
            return ResponseFactory.Ok(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<CommunityMemberDTO>>(ex);
        }
    }

    public async Task<Response<int>> CountMembers(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.communities.Include(x => x.CommunityMembers)
                .ThenInclude(m => m.Member).Where(x => x.Id == communityId).SingleOrDefaultAsync();

            if (community == null) return ResponseFactory.Fail<int>(ErrorsList.CommunityNotFound);

            var cnt = community.CommunityMembers.GroupBy(x => x.MemberId).Count();
            return ResponseFactory.Ok(cnt);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<int>(ex);
        }
    }

    public async Task<Response> JoinRequest(string userId, int communityId)
    {
        try
        {
            var errorsList = new List<Error>();
            var community = await _applicationDbContext.communities.FindAsync(communityId);

            if (community == null) errorsList.Add(ErrorsList.CommunityNotFound);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) errorsList.Add(ErrorsList.CannotFindUser);

            if (errorsList.Any())
                return ResponseFactory.Fail(errorsList);

            await _applicationDbContext.CommunityRequests.AddAsync(new CommunityRequest
            {
                MemberId = userId,
                CommunityId = communityId,
                Status = ConstVariable.PendingStatus
            });
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response<IEnumerable<CommunityMemberDTO>>> GetRequest(int communityId)
    {
        try
        {
            var data = await _applicationDbContext.CommunityRequests.Include(x => x.Member)
                .Where(x => x.CommunityId == communityId && x.Status == ConstVariable.PendingStatus)
                .Select(x => new CommunityMemberDTO
                {
                    Id = x.MemberId,
                    FirstName = x.Member.FirstName,
                    LastName = x.Member.LastName,
                    Email = x.Member.Email,
                    UserName = x.Member.UserName
                }).ToListAsync();
            return ResponseFactory.Ok<IEnumerable<CommunityMemberDTO>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<CommunityMemberDTO>>(ex);
        }
    }

    public async Task<Response> ResponseToRequest(string userId, int communityId, bool accept)
    {
        try
        {
            var request = await _applicationDbContext.CommunityRequests.Where(x =>
                    x.MemberId == userId && x.CommunityId == communityId && x.Status == ConstVariable.PendingStatus)
                .SingleOrDefaultAsync();

            if (request == null) return ResponseFactory.Fail(ErrorsList.JoinRequestNotFound);

            if (accept) request.Status = ConstVariable.AcceptedStatus;
            else request.Status = ConstVariable.RejectedStatus;

            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }
}