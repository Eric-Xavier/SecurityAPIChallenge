using BNPISINClient.Models;

namespace BNPISINClient.Interfaces
{
    public interface ISecurityService
    {
        Task<Dictionary<string,bool>> ExecuteAsync(IEnumerable<string> codeList);
    }
}
