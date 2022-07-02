using System.ComponentModel;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Utility
{
    public class ErrorsList
    {
        public static Error MissingSignUpObject = new Error
            { Code = "MissingSignUpObject", Description = "SignUp object is required" };

        public static Error MissingFirstName = new Error
            { Code = "MissingFirstName", Description = "First Name field is required" };

        public static Error MissingLastName = new Error
            { Code = "MissingLastName", Description = "Last Name field is required" };

        public static Error MissingUsername = new Error
            { Code = "MissingUsername", Description = "Username field is required" };

        public static Error MissingEmail = new Error { Code = "MissingEmail", Description = "Email field is required" };

        public static Error MissingPassword = new Error
            { Code = "MissingPassword", Description = "Password field is required" };

        public static Error MissingConfirmPassword = new Error
            { Code = "MissingConfirmPassword", Description = "Confirm Password field is required" };

        public static Error PasswordDonotMatch = new Error
            { Code = "PasswordDonotMatch", Description = "Confirm Password is not match" };


        public static Error InvalidFirstName = new Error
            { Code = "InvalidFirstName", Description = "Invalid First Name" };

        public static Error InvalidLastName = new Error
            { Code = "InvalidLastName", Description = "Invalid Last Name" };

        public static Error InvalidUsername = new Error
            { Code = "InvalidUsername", Description = "Invalid User Name" };

        public static Error InvalidEmail = new Error { Code = "InvalidEmail", Description = "Invalid email address" };

        //password errors
        public static Error PasswordHasNoNumber = new Error
            { Code = "PasswordHasNoNumber", Description = "Password should contain at least one numeric value" };

        public static Error PasswordHasNoLowerCaseCharacter = new Error
        {
            Code = "PasswordHasNoLowerCaseCharacter",
            Description = "Password should contain at least one lower case letter"
        };

        public static Error PasswordHasNoUpperCaseCharacter = new Error
        {
            Code = "PasswordHasNoUpperCaseCharacter",
            Description = "Password should contain at least one upper case letter"
        };

        public static Error PasswordHasNoSympols = new Error
        {
            Code = "PasswordHasNoSympols", Description = "Password should contain at least one special case characters"
        };

        public static Error PasswordLengthIsTooShort = new Error
            { Code = "PasswordLengthIsTooShort", Description = "Password should not be less than 8 characters" };


        //
        public static Error EmailSenderEmailIsNotConfigured = new Error
            { Code = "EmailSenderEmailIsNotConfigured", Description = "Value cannot be null. (Parameter 'email')" };

        public static Error EmailSenderPasswordIsNotConfigured = new Error
        {
            Code = "EmailSenderPasswordIsNotConfigured", Description = "Value cannot be null. (Parameter 'password')"
        };

        public static Error EmailSenderMailSubmissionPortIsNotConfigured = new Error
        {
            Code = "EmailSenderMailSubmissionPortIsNotConfigured",
            Description = "Value cannot be null. (Parameter 'mailSubmissionPort')"
        };

        public static Error EmailSenderSmtpServerAddressIsNotConfigured = new Error
        {
            Code = "EmailSenderSMTPServerAddressIsNotConfigured",
            Description = "Value cannot be null. (Parameter 'SMTPServerAddress')"
        };

        //Community
        public static Error CommunityNameCanNotBeEmpty = new Error
            { Code = "CommunityNameCanNotBeEmpty", Description = "Value cannot be null or empty. (Parameter 'Name')" };

        public static Error CommunityNotFound = new Error
            { Code = "CommunityNotFound", Description = "Community is not Found" };

        public static Error JoinRequestNotFound = new Error
            { Code = "JoinRequestNotFound", Description = "there is no join request from this user to the community" };

        public static Error RequestNotAcceptOrNotFound = new Error
        {
            Code = "RequestNotAcceptOrNotFound",
            Description = "there user should join the community before assigning role to him"
        };

        //Roles
        public static Error InvalidRoleName = new Error
            { Code = "InvalidRoleName", Description = "Invalid Role Name . it's Not Exited." };

        public static Error DuplicateRoleName = new Error
            { Code = "DuplicateRoleName", Description = "User already has this Role. " };

        public static Error UserHasNotThisRole = new Error
            { Code = "UserHasNotThisRole", Description = "user is not assigned to this role." };

        // weeks
        public static Error WeekNotFound = new Error { Code = "WeekNotFound", Description = "Week is not Found" };
        public static Error SheetNotFound = new Error { Code = "SheetNotFound", Description = "Sheet is not Found" };

        public static Error NoTemplateWeeks = new Error
            { Code = "NoTemplateWeeks", Description = "the  is no Week assigned as template " };

        public static Error NoWeekAvaliable = new Error
            { Code = "NoWeekAvaliable", Description = "no Week Is avaliable " };

        public static Error MissingWeekName = new Error { Code = "MissingWeekName", Description = " Week Is empty " };

        public static Error InvalidWeekName = new Error
            { Code = "InvalidWeekName", Description = "invalid name for this Week" };

        //matiarial
        public static Error MaterailNotFound = new Error
            { Code = "MaterailNotFound", Description = "the material is not exist" };

        public static Error InvalidMaterialName = new Error
            { Code = "MaterailNotFound", Description = "invalid name for this Material" };


        //
        public static Error CannotFindUser = new Error
            { Code = "CannotFindUser", Description = "user is not registered in the system" };

        public static Error IncorrectEmailOrPassword = new Error
            { Code = "IncorrectEmailOrPassword", Description = "can't find a user with this email and password" };

        // training
        public static Error TrainingNotFound = new Error
            { Code = "TrainingNotFound", Description = "training not exists" };

        public static Error InvalidTrainingTitle = new Error
            { Code = "InvalidTrainingTiltle", Description = "invalid title for this Training" };
        public static Error ThereIsAPreviousRequest = new Error
            { Code = nameof(ThereIsAPreviousRequest), Description = "There is a previous request for this user in this training" };
        // week_training bridge
        public static Error DublicateWeekAtTraining = new Error
        {
            Code = "DublicateWeekAtTraining",
            Description = "training already have this week"
        };
        public static Error WeekNotAtTraining = new Error
        {
            Code = "WeekNotAtTraining",
            Description = "training has no relation with week"
        };
        public static Error DublicateSheetAtWeek = new Error
        {
            Code = "DublicateSheetAtWeek",
            Description = "week already have this sheet"
        };
        public static Error SheetNotAtTheWeek = new Error
        {
            Code = "SheetNotAtTheWeek",
            Description = "week have no  relation with sheet"
        };


    }
}
