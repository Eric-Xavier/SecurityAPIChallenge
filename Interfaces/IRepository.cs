using BNPISINClient.Models;

namespace BNPISINClient.Interfaces
{
    public interface IRepository
    {
        Task<bool> Insert(SecurityModel model);

        Task<SecurityModel> GetByISIN(string ISIN);
    }
}
