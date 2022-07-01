namespace ICPC_WebSite_Backend.Data.Models
{
    public class Training
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public bool IsPublic { get; set; }
        public int CommunityId { get; set; }
        public Community Community { get; set; }
        public ICollection<WeekTraining> WeekTraining { get; set; }
        public ICollection<TrainingRequest> TrainingRequests{ get; set; }


    }
}
