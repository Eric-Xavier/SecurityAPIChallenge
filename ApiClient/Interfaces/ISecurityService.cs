using ApiClient.Models;

namespace ApiClient.Interfaces
{
    public interface ISecurityService
    {
        IAsyncEnumerable<ResultModel> ExecuteAsync(IEnumerable<string> codeList);
    }
}
