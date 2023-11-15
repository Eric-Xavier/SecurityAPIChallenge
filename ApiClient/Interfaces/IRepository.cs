using ApiClient.Models;

namespace ApiClient.Interfaces
{
    public interface IRepository
    {
        Task<bool> Insert(SecurityModel model);

        Task<SecurityModel> GetByISIN(string ISIN);
    }
}
