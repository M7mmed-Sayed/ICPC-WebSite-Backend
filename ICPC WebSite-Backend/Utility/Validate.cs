using System.Net.Mail;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Utility;

public static class Validate
{
    public static Response IsValidSignUp(SignUp? signUp)
    {
        if (signUp == null)
            return ResponseFactory.Fail(ErrorsList.MissingSignUpObject);

        var errorList = new List<Error>();

        if (string.IsNullOrEmpty(signUp.FirstName)) errorList.Add(ErrorsList.MissingFirstName);

        if (string.IsNullOrEmpty(signUp.LastName)) errorList.Add(ErrorsList.MissingLastName);

        if (string.IsNullOrEmpty(signUp.UserName)) errorList.Add(ErrorsList.MissingUsername);

        if (string.IsNullOrEmpty(signUp.Email)) errorList.Add(ErrorsList.MissingEmail);

        if (string.IsNullOrEmpty(signUp.Password)) errorList.Add(ErrorsList.MissingPassword);

        if (string.IsNullOrEmpty(signUp.ConfirmPassword)) errorList.Add(ErrorsList.MissingConfirmPassword);

        if (errorList.Any())
            return ResponseFactory.Fail(errorList);

        signUp.FirstName = signUp.FirstName.Trim();
        signUp.LastName = signUp.LastName.Trim();
        signUp.UserName = signUp.UserName.Trim();
        signUp.Email = signUp.Email.Trim();
        signUp.Password = signUp.Password.Trim();
        signUp.ConfirmPassword = signUp.ConfirmPassword.Trim();

        if (!IsValidName(signUp.FirstName)) errorList.Add(ErrorsList.InvalidFirstName);

        if (!IsValidName(signUp.LastName)) errorList.Add(ErrorsList.InvalidLastName);

        if (!IsValidUserName(signUp.UserName)) errorList.Add(ErrorsList.InvalidUsername);

        if (!IsValidEmail(signUp.Email)) errorList.Add(ErrorsList.InvalidEmail);

        var validatePassword = IsStrongPassword(signUp.Password, signUp.ConfirmPassword);

        if (!validatePassword.Succeeded) errorList.AddRange(validatePassword.Errors);

        return ResponseFactory.ResponseFromErrors(errorList);
    }

    public static Response IsValidCommunity(CommunityDto community)
    {
        var errorList = new List<Error>();

        if (string.IsNullOrEmpty(community.Name)) errorList.Add(ErrorsList.CommunityNameCanNotBeEmpty);

        if (!IsValidEmail(community.OfficialMail)) errorList.Add(ErrorsList.InvalidEmail);

        return ResponseFactory.ResponseFromErrors(errorList);
    }

    private static bool IsValidName(string name) => RegexPattrens.Name.IsMatch(name);

    private static bool IsValidUserName(string username) => RegexPattrens.Username.IsMatch(username);

    public static bool IsValidEmail(string email)
    {
        try
        {
            if (email.EndsWith(".")) return false;
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static Response IsStrongPassword(string password, string confirmPassword = "")
    {
        var errorList = new List<Error>();
        if (!RegexPattrens.HasNumber.IsMatch(password)) errorList.Add(ErrorsList.PasswordHasNoNumber);

        if (!RegexPattrens.HasLowerChar.IsMatch(password)) errorList.Add(ErrorsList.PasswordHasNoLowerCaseCharacter);

        if (!RegexPattrens.HasUpperChar.IsMatch(password)) errorList.Add(ErrorsList.PasswordHasNoUpperCaseCharacter);

        if (!RegexPattrens.HasSymbols.IsMatch(password)) errorList.Add(ErrorsList.PasswordHasNoSympols);

        if (!RegexPattrens.HasMinimum8Chars.IsMatch(password)) errorList.Add(ErrorsList.PasswordLengthIsTooShort);

        if (!password.Equals(confirmPassword)) errorList.Add(ErrorsList.PasswordDonotMatch);

        return ResponseFactory.ResponseFromErrors(errorList);
    }

    public static Response IsValidMaterial(string material) => !RegexPattrens.Username.IsMatch(material)
        ? ResponseFactory.Fail(ErrorsList.InvalidMaterialName)
        : ResponseFactory.Ok();

    public static Response IsValidWeek(WeekDto weekDTO)
    {
        if (String.IsNullOrEmpty(weekDTO.Name))
        {
            return ResponseFactory.Fail(ErrorsList.InvalidWeekName);
        }

        return ResponseFactory.Ok();
    }

    public static Response IsValidTraining(TrainingDTO trainingDTO)
    {
        if (String.IsNullOrEmpty(trainingDTO.Title))
        {
            return ResponseFactory.Fail(ErrorsList.InvalidTrainingTitle);
        }

        return ResponseFactory.Ok();
    }
}