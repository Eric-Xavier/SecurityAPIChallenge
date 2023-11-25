using ApiClient.Interfaces;
using ApiClient.Models;
using Microsoft.EntityFrameworkCore;
using ApiClient.Services.Repository.Mapper;


namespace ApiClient.Services
{
    /// <summary>
    /// Implementation for database based on EntityFramwork
    /// </summary>
    public class RepositoryContext : DbContext, IRepository
    {
        public DbSet<SecurityModel> Securities { get; set; }
        private ILogger<RepositoryContext> _logger;

        public const string SqlServerConnectionPropertyName = "SqlServer";


        public RepositoryContext(DbContextOptions<RepositoryContext> options, ILogger<RepositoryContext> logger)
            :base(options) => _logger = logger;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfiguration(new SecurityEntityConfigurationMapper());


        public async Task<SecurityModel?> GetByISIN(string ISIN)
            => await Securities.FirstOrDefaultAsync(x=>x.ISINCode == ISIN);
        

        public async Task<bool> Insert(SecurityModel model)
        {
            try
            {
                Securities.Add(model);
                var affectedRows = await SaveChangesAsync();

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Insert fail for @model", model);
                return false;
            }
        }
    
    }
}

