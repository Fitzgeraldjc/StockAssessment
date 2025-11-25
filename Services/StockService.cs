using System.Text.Json;
using StockAssessment.Models;

namespace StockAssessment.Services;

public class StockService : IStockService
{
    private readonly HttpClient _httpClient;

    public StockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DailyStockSummary>> GetMonthlyDataAsync(
        string symbol,
        string? userApiKey
    )
    {
        var freeKey = "0KCRJMPH18QK2HG5";

        bool isPremium = !string.IsNullOrEmpty(userApiKey);

        var finalApiKey = isPremium ? userApiKey : freeKey;

        var sizeParam = isPremium ? "&outputsize=full" : "";
        var url =
            $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=15min{sizeParam}&apikey={finalApiKey}";

        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var rawData = JsonSerializer.Deserialize<AlphaVantageResponse>(content);

        if (rawData?.TimeSeries == null)
        {
            return new List<DailyStockSummary>();
        }

        var query =
            from kvp in rawData.TimeSeries
            let timestamp = DateTime.Parse(kvp.Key)
            let high = double.Parse(kvp.Value.High)
            let low = double.Parse(kvp.Value.Low)
            let volume = long.Parse(kvp.Value.Volume)

            group new
            {
                high,
                low,
                volume,
            } by timestamp.Date into dailyGroup

            orderby dailyGroup.Key descending

            select new DailyStockSummary
            {
                Day = dailyGroup.Key.ToString("yyyy-MM-dd"),

                LowAverage = Math.Round(dailyGroup.Average(x => x.low), 4),
                HighAverage = Math.Round(dailyGroup.Average(x => x.high), 4),

                Volume = dailyGroup.Sum(x => x.volume),
            };

        return query.ToList();
    }
}
