using ApiClient.Models;

namespace ApiClient.Interfaces
{
    public interface ISecurityService
    {
        Task<List<KeyValuePair<string, bool>>> ExecuteAsync(IEnumerable<string> codeList);
    }
}
