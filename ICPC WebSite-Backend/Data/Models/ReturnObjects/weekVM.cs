namespace ICPC_WebSite_Backend.Data.Models.ReturnObjects
{
    public class weekVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTemplate { get; set; } = false;
    }
}
