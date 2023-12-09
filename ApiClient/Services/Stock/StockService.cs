
using ApiClient.Models;
using ApiClient.Interfaces;

namespace ApiClient.Services.Stock
{
    public class StockService  : IStockService
    {
        readonly HttpClient _httpClient;
        readonly ILogger<StockService> _logger;
        public const string ServiceApiUrlConfigName = "ServiceApiBaseUrl";

        public StockService(HttpClient httpClient, ILogger<StockService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<SecurityModel?> GetSecurityPrice(string security)
        {
            try
            {
                return await _httpClient
                    .GetFromJsonAsync<SecurityModel>($"securityprice/{security}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ISIN:{security} not found in webservice");
                return null;
            } 
            
        }

    }

}
