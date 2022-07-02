using Microsoft.AspNetCore.Identity;

namespace UtilityLibrary.Response;

public static class IdentityResultExtensions
{
    public static Response ToApplicationResponse(this IdentityResult identityResult)
    {
        var errors = identityResult.Errors.Select(e => new Error {Code = e.Code, Description = e.Description});
        return new Response(identityResult.Succeeded, errors);
    }
    
    public static Response<T> ToApplicationResponse<T>(this IdentityResult identityResult, T? data = default)
    {
        var errors = identityResult.Errors.Select(e => new Error {Code = e.Code, Description = e.Description});
        return new Response<T>(identityResult.Succeeded, default, errors);
    }

    public static Response ToApplicationResponse(this SignInResult signInResult) => new(signInResult.Succeeded, default);

    public static Response<T> ToApplicationResponse<T>(this SignInResult signInResult, T? data = default) =>
        new Response<T>(signInResult.Succeeded, default, default);
}