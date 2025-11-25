# Stock Data Aggregator API

A .NET Web API that retrieves intraday stock data from the Alpha Vantage API, processes it into daily summaries, and exposes it via a REST endpoint.

## Overview

This application queries the Alpha Vantage `TIME_SERIES_INTRADAY` endpoint (15-minute intervals), groups the data by day, and calculates the following statistics for each date:
* Average Low price
* Average High price
* Total Volume

The API is built to handle the external API's JSON structure (dynamic date keys) and provides a clean JSON response to the client.

## Tech Stack

* .NET 8 Web API
* C#
* System.Text.Json
* LINQ (Query Syntax)

## Setup and Running

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/Fitzgeraldjc/StockAssessment.git](https://github.com/Fitzgeraldjc/StockAssessment.git)
    cd StockAssessment
    ```

2.  **Run the application**
    ```bash
    dotnet watch run
    ```

3.  **Access the API**
    Once running, the Swagger UI is available at the local URL (typically `http://localhost:5xxx/swagger`) for testing.

## Usage

### Get Daily Summary
`GET /api/stock/{symbol}`

**Parameters:**
* `symbol` (Required): The stock ticker (e.g., IBM, AAPL).
* `apiKey` (Optional): A personal Alpha Vantage API key. If omitted, the application defaults to a free demo key (which limits data to the last 100 points).

**Example Request:**
```http
GET http://localhost:5xxx/api/stock/IBM
GET http://localhost:5xxx/api/stock/IBM/?apiKey=YOUR_API_KEY
