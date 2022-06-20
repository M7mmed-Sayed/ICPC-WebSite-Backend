namespace ICPC_WebSite_Backend.Data.Models
{
    public class Week
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTemplate { get; set; }=false;
        public  DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public List<Matirial> matirials { get; set; }
        public int Training_Id { get; set; }
        public Training Training { get; set; }
    }
}
