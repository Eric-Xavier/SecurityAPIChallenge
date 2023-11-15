using ApiClient.Interfaces;
using ApiClient.Models;

namespace ApiClient.Services
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
