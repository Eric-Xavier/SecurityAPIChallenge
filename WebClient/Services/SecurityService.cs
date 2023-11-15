using BNPISINClient.Interfaces;
using BNPISINClient.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static System.Net.WebRequestMethods;

namespace BNPISINClient.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly HttpClient _httpClient;
        private IRepository _repository { get; set; }
        private ILogger<SecurityService> _logger { get; set; }
        private string apiURL { get; }

        public const string ServiceApiUrlConfigName = "ServiceApiUrl";


        public SecurityService(HttpClient httpClient, IRepository repository, ILogger<SecurityService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
            apiURL = configuration.GetValue<string>(ServiceApiUrlConfigName);
        }

        private async Task<bool> ExecuteAsync(string ISIN) {

            if(!ValidateSecurityCode(ISIN))
                return false;
            

            var apiResponse = await _httpClient
                .GetFromJsonAsync<SecurityModel>(apiURL.Replace("{isin}", ISIN));


            if (apiResponse == null)
            {
                _logger.LogError("ISIN not found");
                return false;
            }

            
            var isStored = await _repository.Insert(apiResponse);
            if (!isStored)
            {
                _logger.LogError("Not Stored on database");
                return false;
            }

            return true;

        }

        public async Task<Dictionary<string, bool>> ExecuteAsync(IEnumerable<string> codeList)
        {
            var securityCodeProcessed = new Dictionary<string, bool>();
            
            foreach (var code in codeList.Where(c=>c != null))
                securityCodeProcessed.Add(code, await ExecuteAsync(code));
            
            return securityCodeProcessed;
        }

        private bool ValidateSecurityCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code)
                && code.Length == 12;
        }

    }
}
