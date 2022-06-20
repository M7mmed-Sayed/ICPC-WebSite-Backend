namespace ICPC_WebSite_Backend.Data.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public int weekId { get; set; }
        public Week weeks { get; set; }
    }
}
