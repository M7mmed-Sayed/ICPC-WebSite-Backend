using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Models.DTO;

namespace ICPC_WebSite_Backend.Utility
{
    public static class Validate
    {
        public static Response IsValidSignUp(SignUp signUp) {
            var result = new Response();
            if (signUp == null) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingSignUpObject);
                return result;
            }
            if (string.IsNullOrEmpty(signUp.FirstName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingFirstName);
            }
            if (string.IsNullOrEmpty(signUp.LastName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingLastName);
            }
            if (string.IsNullOrEmpty(signUp.UserName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingUsername);
            }
            if (string.IsNullOrEmpty(signUp.Email)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingEmail);
            }
            if (string.IsNullOrEmpty(signUp.Password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingPassword);
            }
            if (string.IsNullOrEmpty(signUp.ConfirmPassword)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.MissingConfirmPassword);
            }
            if (!result.Succeeded)
                return result;
            signUp.FirstName = signUp.FirstName.Trim();
            signUp.LastName = signUp.LastName.Trim();
            signUp.UserName = signUp.UserName.Trim();
            signUp.Email = signUp.Email.Trim();
            signUp.Password = signUp.Password.Trim();
            signUp.ConfirmPassword = signUp.ConfirmPassword.Trim();

            if (!IsValidName(signUp.FirstName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidFirstName);
            }
            if (!IsValidName(signUp.LastName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidLastName);
            }
            if (!IsValidUserName(signUp.UserName)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidUsername);
            }
            if (!IsValidEmail(signUp.Email)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidEmail);
            }
            var validatePassword = IsStrongPassword(signUp.Password, signUp.ConfirmPassword);
            if (!validatePassword.Succeeded) {
                result.Succeeded = false;
                result.Errors.AddRange(validatePassword.Errors);
            }
            return result;
        }
        public static Response IsValidCommunity(CommunityDTO community) {
            var result = new Response();
            if (string.IsNullOrEmpty(community.Name)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.CommunityNameCanNotBeEmpty);
            }
            if (!IsValidEmail(community.OfficialMail)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidEmail);
            }
            return result;
        }
        public static bool IsValidName(string name) {
            if (!RegexPattrens.Name.IsMatch(name)) {
                return false;
            }
            return true;
        }
        public static bool IsValidUserName(string username) {
            if (!RegexPattrens.Username.IsMatch(username)) {
                return false;
            }
            return true;
        }
        public static bool IsValidEmail(string email) {
            try {
                if (email.EndsWith(".")) {
                    return false;
                }
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
        public static Response IsStrongPassword(string password, string confirmPassword = "") {
            var result = new Response();
            if (!RegexPattrens.HasNumber.IsMatch(password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordHasNoNumber);
            }
            if (!RegexPattrens.HasLowerChar.IsMatch(password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordHasNoLowerCaseCharacter);
            }
            if (!RegexPattrens.HasUpperChar.IsMatch(password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordHasNoUpperCaseCharacter);
            }
            if (!RegexPattrens.HasSymbols.IsMatch(password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordHasNoSympols);
            }
            if (!RegexPattrens.HasMinimum8Chars.IsMatch(password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordLengthIsTooShort);
            }
            if (!password.Equals(confirmPassword)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.PasswordDonotMatch);
            }
            return result;

        }
    }
}
