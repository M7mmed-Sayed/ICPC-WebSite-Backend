using ICPC_WebSite_Backend.Models;

namespace ICPC_WebSite_Backend.Utility
{
    public class ErrorsList
    {
        public static Error MissingSignUpObject = new Error { Code = "MissingSignUpObject", Description = "SignUp object is required" };
        public static Error MissingFirstName = new Error { Code = "MissingFirstName", Description = "First Name field is required" };
        public static Error MissingLastName = new Error { Code = "MissingLastName", Description = "Last Name field is required" };
        public static Error MissingUsername = new Error { Code = "MissingUsername", Description = "Username field is required" };
        public static Error MissingEmail = new Error { Code = "MissingEmail", Description = "Email field is required" };
        public static Error MissingPassword = new Error { Code = "MissingPassword", Description = "Password field is required" };
        public static Error MissingConfirmPassword = new Error { Code = "MissingConfirmPassword", Description = "Confirm Password field is required" };
        public static Error PasswordDonotMatch = new Error { Code = "PasswordDonotMatch", Description = "Confirm Password field is required" };


        public static Error InvalidFirstName = new Error { Code = "InvalidFirstName", Description = "Invalid email address" };
        public static Error InvalidLastName = new Error { Code = "InvalidLastName", Description = "Invalid email address" };
        public static Error InvalidUsername = new Error { Code = "InvalidUsername", Description = "Invalid email address" };
        public static Error InvalidEmail = new Error { Code = "InvalidEmail", Description = "Invalid email address" };

        //password errors
        public static Error PasswordHasNoNumber = new Error { Code = "PasswordHasNoNumber", Description = "Password should contain at least one numeric value" };
        public static Error PasswordHasNoLowerCaseCharacter = new Error { Code = "PasswordHasNoLowerCaseCharacter", Description = "Password should contain at least one lower case letter" };
        public static Error PasswordHasNoUpperCaseCharacter = new Error { Code = "PasswordHasNoUpperCaseCharacter", Description = "Password should contain at least one upper case letter" };
        public static Error PasswordHasNoSympols = new Error { Code = "PasswordHasNoSympols", Description = "Password should contain at least one special case characters" };
        public static Error PasswordLengthIsTooShort = new Error { Code = "PasswordLengthIsTooShort", Description = "Password should not be less than 8 characters" };


        //
        public static Error EmailSenderEmailIsNotConfigured = new Error { Code = "EmailSenderEmailIsNotConfigured", Description = "Value cannot be null. (Parameter 'email')" };
        public static Error EmailSenderPasswordIsNotConfigured = new Error { Code = "EmailSenderPasswordIsNotConfigured", Description = "Value cannot be null. (Parameter 'password')" };
        public static Error EmailSenderMailSubmissionPortIsNotConfigured = new Error { Code = "EmailSenderMailSubmissionPortIsNotConfigured", Description = "Value cannot be null. (Parameter 'mailSubmissionPort')" };
        public static Error EmailSenderSMTPServerAddressIsNotConfigured = new Error { Code = "EmailSenderSMTPServerAddressIsNotConfigured", Description = "Value cannot be null. (Parameter 'SMTPServerAddress')" };

        //Community
        public static Error CommunityNameCanNotBeEmpty = new Error { Code = "CommunityNameCanNotBeEmpty", Description = "Value cannot be null or empty. (Parameter 'Name')" };
    }
}
