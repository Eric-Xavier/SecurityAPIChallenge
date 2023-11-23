using ApiClient.Helper;
using ApiClient.Interfaces;


namespace ApiClient.Services
{
    public class SecurityService : ISecurityService
    {
        private IRepository _repository { get; init; }
        private ILogger<SecurityService> _logger { get; init; }
        private IStockService _stockService { get; init; }
        public const string ServiceApiUrlConfigName = "ServiceApiUrl";



        public SecurityService(IRepository repository, IStockService stockService, ILogger<SecurityService> logger)
        {
            _repository = repository;
            _stockService = stockService;
            _logger = logger;
        }

        private async Task<bool> ExecuteAsync(string ISIN)
        {
            try
            {
                if (!ValidatorHelper.ValidateSecurityCode(ISIN))
                    return false;

                var apiResponse = await _stockService.GetSecurityPrice(ISIN);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Process ISIN:{isin}", ISIN);
                return false;
            }

        }

        public async Task<List<KeyValuePair<string, bool>>> ExecuteAsync(IEnumerable<string> codeList)
        {
            var securityCodeProcessed = new List<KeyValuePair<string, bool>>();

            foreach (var code in codeList)
            {
                var isProcessed = await ExecuteAsync(code);
                securityCodeProcessed.Add(new KeyValuePair<string, bool>(code, isProcessed));
            }

            return securityCodeProcessed;
        }

    }
}
