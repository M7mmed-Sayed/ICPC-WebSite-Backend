namespace ICPC_WebSite_Backend.Data.Models;

public class TrainingRequest
{
    public string MemberId { get; set; }
    public User Member { get; set; }
    public int TrainingId { get; set; }
    public Training Training { get; set; }
    public string Status { get; set; }
    
}