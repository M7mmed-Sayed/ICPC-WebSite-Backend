using System.Security.Claims;
using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

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
        ResponseFactory.Ok<IEnumerable<Community>>(await _applicationDbContext.Communities.ToListAsync());

    public async Task<Response<Community>> GetCommunity(int id)
    {
        var data = await _applicationDbContext.Communities.FindAsync(id);
        return data is null
            ? ResponseFactory.Fail<Community>(ErrorsList.CommunityNotFound)
            : ResponseFactory.Ok(data);
    }

    public async Task<Response> RegisterCommunityAsync(CommunityDto communityDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(communityDto.RequesterId);

            if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);
            if (!await _roleManager.RoleExistsAsync(RolesList.CommunityLeader))
                return ResponseFactory.Fail(ErrorsList.InvalidRoleName);
            if (await _userManager.IsInRoleAsync(user, RolesList.CommunityLeader))
                return ResponseFactory.Fail(ErrorsList.UserHaveSameRole);
            var result = await _userManager.AddToRoleAsync(user, RolesList.CommunityLeader);
            if (result.Succeeded == false) return result.ToApplicationResponse();
            var community = new Community
            {
                Name = communityDto.Name,
                About = communityDto.About,
                OfficialMail = communityDto.OfficialMail,
                RequesterId = communityDto.RequesterId,
                IsApproved = true
            };
            await _applicationDbContext.Communities.AddAsync(community);
            await _applicationDbContext.SaveChangesAsync();
            var message = $"congratulations {user.FirstName} <br>";
            message += $"Your request for Creating {community.Name} Community was Accepted" +
                       $" <br> Welcome again for you and for {community.Name} community mempers's  <br>" +
                       $"We assigned you to be {community.Name} Leader "
                ;
            var subject = "Competitive Programing Registeration Community";
            var emailSendResult = _emailSender.SendEmail(user.Email, subject, message);
            return emailSendResult.Succeeded ? ResponseFactory.Ok(community.Id) : emailSendResult;
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> EditCommunity(int communityId, CommunityDto communityDto)
    {
        try
        {
            var community = await _applicationDbContext.Communities.FindAsync(communityId);
            if (community == null)
                return ResponseFactory.Fail(ErrorsList.CommunityNotFound);
            community.About = communityDto.About;
            community.Name = communityDto.Name;
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok(community.Id);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }
    public async Task<Response> DeleteCommunity(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.Communities.FindAsync(communityId);
            if (community == null)
                return ResponseFactory.Fail(ErrorsList.CommunityNotFound);
          
            _applicationDbContext.Remove(community);
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
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
            var community = await _applicationDbContext.Communities.FindAsync(communityId);

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
                var addClaimResult=await AddClaimsToUserAsync(user, ClaimsNames.CommunityIdClaimName, community.Id.ToString());
                if (!addClaimResult.Succeeded) return ResponseFactory.Fail( addClaimResult.Errors!);
            }
            await _applicationDbContext.SaveChangesAsync();

            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response<IEnumerable<CommunityMemberDto>>> GetMembers(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.Communities.FindAsync(communityId);

            if (community == null)
                return ResponseFactory.Fail<IEnumerable<CommunityMemberDto>>(ErrorsList.CommunityNotFound);

            var data = community.Members.Select(member=>new  CommunityMemberDto
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    UserName = member.UserName,
                }).ToList();
            return ResponseFactory.Ok<IEnumerable<CommunityMemberDto>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<CommunityMemberDto>>(ex);
        }
    }

    public async Task<Response<int>> CountMembers(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.Communities.FindAsync(communityId);

            if (community == null) return ResponseFactory.Fail<int>(ErrorsList.CommunityNotFound);

            var cnt = community.Members.Count;
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
            var community = await _applicationDbContext.Communities.FindAsync(communityId);

            if (community == null) errorsList.Add(ErrorsList.CommunityNotFound);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) errorsList.Add(ErrorsList.CannotFindUser);

            if (errorsList.Any())
                return ResponseFactory.Fail(errorsList);

            var previousRequest =await  _applicationDbContext.CommunityRequests.Where(x => x.MemberId == userId).FirstOrDefaultAsync();
            if (previousRequest != null)
            {
                return ResponseFactory.Fail(ErrorsList.ThereIsAPreviousRequestForThisUser);
            }
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

    public async Task<Response<IEnumerable<CommunityMemberDto>>> GetRequest(int communityId)
    {
        try
        {
            var data = await _applicationDbContext.CommunityRequests.Include(x => x.Member)
                .Where(x => x.CommunityId == communityId && x.Status == ConstVariable.PendingStatus)
                .Select(x => new CommunityMemberDto
                {
                    Id = x.MemberId,
                    FirstName = x.Member.FirstName,
                    LastName = x.Member.LastName,
                    Email = x.Member.Email,
                    UserName = x.Member.UserName
                }).ToListAsync();
            return ResponseFactory.Ok<IEnumerable<CommunityMemberDto>>(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<CommunityMemberDto>>(ex);
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
            var user = await _userManager.FindByIdAsync(request.MemberId);
            if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);
            var community = await _applicationDbContext.Communities.FindAsync(communityId);
            if (community == null) return ResponseFactory.Fail(ErrorsList.CommunityNotFound);
            if (accept)
            {
                request.Status = ConstVariable.AcceptedStatus;
                var addClaimResult=await AddClaimsToUserAsync(user, ClaimsNames.CommunityIdClaimName, community.Id.ToString());
                if (!addClaimResult.Succeeded) return ResponseFactory.Fail( addClaimResult.Errors!);
                user.CommunityId = community.Id;

            }
            else
            {
                //request.Status = ConstVariable.RejectedStatus;
                _applicationDbContext.CommunityRequests.Remove(request);
            }

            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    private async Task<Response> AddClaimsToUserAsync(User user,string claimName,string claimValue)
    {
        var previousClaim =  (await  _userManager.GetClaimsAsync(user)).FirstOrDefault(claim => claim.Type==ClaimsNames.CommunityIdClaimName);
        if (previousClaim!=null)await _userManager.RemoveClaimAsync(user,previousClaim);
                
        var claimsResult = await _userManager.AddClaimAsync(user, new Claim(claimName, claimValue));
        return claimsResult.Succeeded ? ResponseFactory.Ok():  claimsResult.ToApplicationResponse<Response>();
    }
}