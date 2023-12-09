using System;
using System.Threading.Tasks;
using ApiClient.Models;

namespace ApiClient.Interfaces
{
    public interface IStockService
    {
        Task<SecurityModel?> GetSecurityPrice(string security);
    }
}