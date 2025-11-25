namespace StockAssessment.Models;

public class DailyStockSummary
{
    public string Day { get; set; }
    public double LowAverage { get; set; }
    public double HighAverage { get; set; }
    public long Volume { get; set; }
}
