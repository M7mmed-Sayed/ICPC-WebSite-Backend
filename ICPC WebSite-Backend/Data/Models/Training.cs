namespace ICPC_WebSite_Backend.Data.Models
{
    public class Training
    {
        public DateTime Created_At { get; set; }
        public DateTime Upated_At { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public bool IsPublic { get; set; }
        public int Community_Id { get; set; }
        public Community Community{ get; set; }

    }
}
