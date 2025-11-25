using StockAssessment.Models;

namespace StockAssessment.Services;

public interface IStockService
{
    Task<List<DailyStockSummary>> GetMonthlyDataAsync(string symbol, string? apiKey = null);
}
