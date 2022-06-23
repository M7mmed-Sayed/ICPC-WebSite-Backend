namespace ICPC_WebSite_Backend.Data.Models;

public class WeekTraining
{
    public int WeekId { get; set; }
    public Week Week { get; set; }
    public int TrainingId { get; set; }
    public Training Training { get; set; }
}