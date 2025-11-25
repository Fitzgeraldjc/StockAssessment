using StockAssessment.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Register the Service
builder.Services.AddHttpClient<IStockService, StockService>();

// Add this if you want swagger (optional but good for testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. Define the Endpoint
app.MapGet(
        "/api/stock/{symbol}",
        async (string symbol, IStockService stockService, string? apiKey = null) =>
        {
            try
            {
                var data = await stockService.GetMonthlyDataAsync(symbol, apiKey);

                if (data.Count == 0)
                    return Results.NotFound("No data found. Check symbol or API limits.");

                return Results.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    )
    .WithName("GetStockData");

app.Run();
