
using Microsoft.Extensions.Options;
using ApiClient.Models;
using ApiClient.Interfaces;

namespace ApiClient.Services
{
    public class StockService  : IStockService
    {
        HttpClient _httpClient;
        public const string ServiceApiUrlConfigName = "ServiceApiBaseUrl";

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<SecurityModel> GetSecurityPrice(string security)
        {
            return await _httpClient
                    .GetFromJsonAsync<SecurityModel>($"securityprice/{security}");
        }


    }

}
