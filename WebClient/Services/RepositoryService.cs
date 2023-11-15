using BNPISINClient.Interfaces;
using BNPISINClient.Models;

namespace BNPISINClient.Services
{
    public class RepositoryService : IRepository
    {
        public Task<SecurityModel> GetByISIN(string ISIN)
        {
            return Task.FromResult(new SecurityModel());
        }

        public Task<bool> Insert(SecurityModel model)
        {
            return Task.FromResult(true);
        }
    }
}
