using ApiClient.Models;

namespace ApiClient.Interfaces
{
    public interface ISecurityService
    {
        Task<Dictionary<string,bool>> ExecuteAsync(IEnumerable<string> codeList);
    }
}
