namespace ICPC_WebSite_Backend.Utility
{
    public class RolesList
    {
        public const string Administrator = "Admin";
        public const string CommunityLeader = "CommunityLeader";
        public const string HeadOfTraining = "HeadOfTraining";
        public const string TrainingManager = "TrainingManager";
        public const string Mentor = "Mentor";
        public const string Trainee = "Trainee";
        public static List<string> Roles { get; private set; } = new List<string>() { Administrator, CommunityLeader, HeadOfTraining, TrainingManager, Mentor, Trainee };

    }
}
