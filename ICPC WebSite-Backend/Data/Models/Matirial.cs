namespace ICPC_WebSite_Backend.Data.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int WeekId { get; set; }
        public Week Weeks { get; set; }
    }
}
