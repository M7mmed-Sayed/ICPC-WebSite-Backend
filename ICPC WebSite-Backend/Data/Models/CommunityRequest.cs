namespace ICPC_WebSite_Backend.Data.Models
{
    public class CommunityRequest
    {
        public string MemberId { get; set; }
        public User Member { get; set; }
        public int CommunityId { get; set; }
        public Community Community { get; set; }
        public string Status { get; set; }
    }
}
