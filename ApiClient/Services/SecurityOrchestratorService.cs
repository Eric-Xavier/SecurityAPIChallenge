using ApiClient.Helper;
using ApiClient.Interfaces;
using ApiClient.Models;
using ApiClient.Models.Enums;


namespace ApiClient.Services
{
    public class SecurityOrchestratorService : ISecurityService
    {
        private IRepository _repository { get; init; }
        private ILogger<SecurityOrchestratorService> _logger { get; init; }
        private IStockService _stockService { get; init; }
        public const string ServiceApiUrlConfigName = "ServiceApiUrl";



        public SecurityOrchestratorService(IRepository repository, IStockService stockService, ILogger<SecurityOrchestratorService> logger)
        {
            _repository = repository;
            _stockService = stockService;
            _logger = logger;
        }

        public async IAsyncEnumerable<ResultModel> ExecuteAsync(IEnumerable<string> codeList)
        {

            foreach (var code in codeList)
            {
                if (!ValidatorHelper.ValidateSecurityCode(code)){
                    yield return new ResultModel(code, ErrorCodes.NotValidSecurityCode.ToString());
                    continue;
                }

                var apiResponse = await _stockService.GetSecurityPrice(code);
                if (apiResponse == null){
                    yield return new ResultModel(code, ErrorCodes.NotFoundInWebService.ToString());
                    continue;
                }

                if (!await _repository.Insert(apiResponse)){
                    yield return new ResultModel(code, ErrorCodes.NotStoredInDatabase.ToString());
                    continue;
                }

                yield return new ResultModel(code, ErrorCodes.NoError.ToString());

            }
        }

    }
}
