﻿using ApiClient.Interfaces;
using ApiClient.Models;
using System.Text.Json;

namespace ApiClient.Services
{
    public class SecurityService : ISecurityService
    {
        private HttpClient _httpClient;
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
            try
            {
                if (!ValidateSecurityCode(ISIN))
                    return false;

                //workaround
                _httpClient = new HttpClient(new FakeResponse());

                var apiResponse = await _httpClient
                    .GetFromJsonAsync<SecurityModel>(apiURL.Replace("{isin}", ISIN));


                if (apiResponse == null)
                {
                    _logger.LogError("ISIN not found in webservice");
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to Process ISIN:{isin}", ISIN);
                return false;
            }

        }

        public async Task<Dictionary<string, bool>> ExecuteAsync(IEnumerable<string> codeList)
        {
            var securityCodeProcessed = new Dictionary<string, bool>();
            
            foreach (var code in codeList.Where(c=>c != null))
                securityCodeProcessed.Add(code, await ExecuteAsync(code));
            
            return securityCodeProcessed;
        }

        private static bool ValidateSecurityCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code)
                && code.Length == 12;
        }

    }

    /// <summary>
    /// @todo: remove as soon as implement json-server component
    /// </summary>
    internal class FakeResponse : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(CreateHttpResponse(request?.RequestUri?.Segments?.Last() ?? "abcdefghjkl"));
        }

        private HttpResponseMessage CreateHttpResponse(string id)
        {
            var response = new SecurityModel { 
                ISINCode= id ,
                Price = decimal.Parse($"{Random.Shared.Next(1000, 99999)},{Random.Shared.Next(0, 99)}") 
            };

            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonContent.Create<SecurityModel>(response)
            };
        }
    }
}
