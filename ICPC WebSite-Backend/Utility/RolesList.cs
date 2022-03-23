namespace ICPC_WebSite_Backend.Utility
{
    public class RolesList
    {
        public static string Administrator = "Admin";
        public static string CommunityLeader = "CommunityLeader";
        public static string HeadOfTraining = "HeadOfTraining";
        public static string TrainingManager = "TrainingManager";
        public static string Mentor = "Mentor";
        public static string Trainee = "Trainee";
        public static List<string> Roles { get; set; } = new List<string>() { Administrator, CommunityLeader, HeadOfTraining, TrainingManager, Mentor, Trainee };

    }
}
