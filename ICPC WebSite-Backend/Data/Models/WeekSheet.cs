namespace ICPC_WebSite_Backend.Data.Models;

public class WeekSheet
{
    public int WeekId { get; set; }
    public Week Week { get; set; }
    
    public int SheetId { get; set; }
    public Sheet Sheet { get; set; }
}