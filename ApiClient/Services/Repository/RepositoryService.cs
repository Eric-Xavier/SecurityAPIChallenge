using ApiClient.Interfaces;
using ApiClient.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace ApiClient.Services
{
    //@todo:https://dotnet.testcontainers.org/examples/aspnet/

    /// <summary>
    /// Implementation for a repository based on Dapper with custom queries
    /// </summary>
    public class RepositoryService : IRepository
    {
        public const string SqlServerConnectionPropertyName = "SqlServer";
        private readonly string _connectionString;
        private readonly ILogger<RepositoryService> _logger;

        public RepositoryService(IConfiguration config, ILogger<RepositoryService> logger)
        {
            _connectionString = config.GetConnectionString(SqlServerConnectionPropertyName) ?? 
                throw new ArgumentNullException(nameof(config));

            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }




        public async Task<SecurityModel> GetByISIN(string ISIN)
        {

            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@isin", ISIN, DbType.String);
                
                var result = await connection.QueryFirstAsync<SecurityModel>("SELECT isin, price FROM Securities WHERE isin = @isin;");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on quering {isin} from the Database", ISIN);
                return null;
            }

        }

        public async Task<bool> Insert(SecurityModel model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@isin", model.ISINCode, DbType.String);
                parameters.Add("@price", model.Price, DbType.Decimal);
                

                var result = await connection.ExecuteScalarAsync("INSERT INTO Securities (isin, price) VALUES (@isin, @price);  SELECT @@ROWCOUNT;", parameters);
                return result != null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data no stored in Database for {isin}", model.ISINCode);
                return false;
            }
        }


        public async Task<bool> Update(SecurityModel model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@isin", model.ISINCode, DbType.String);
                parameters.Add("@price", model.Price, DbType.Decimal);

                var result = await connection.ExecuteScalarAsync("UPDATE Securities isin=@isin, price=@price WHERE isin = @isin;");
                return result != null;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No data was updated in Database for {isin}", model.ISINCode);
                return false;
            }
        }

    }
}
