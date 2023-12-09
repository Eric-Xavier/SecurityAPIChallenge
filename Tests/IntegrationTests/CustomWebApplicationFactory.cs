
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using ApiClient.Interfaces;
using ApiClient.Services.Stock;
using Microsoft.Extensions.DependencyInjection;
using ApiClient.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using ApiClient.Services.Repository;
using Testcontainers.MsSql;
using DotNet.Testcontainers.Containers;

namespace IntegrationTests;
public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{

    public MsSqlContainer DataBaseContainer { get; private set; }
    public DockerContainer WebserviceContainer { get; private set; }

    const string ServiceApiBaseUrl = "http://localhost:4000";
    IConfiguration? configuration;


    public CustomWebApplicationFactory()
    {
        DataBaseContainer = new DatabaseInContainerFixture().msSqlContainer;
        WebserviceContainer = new SecuritiesDataProviderFixture().Container;
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Development");


        builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(GetAppSettings());
            configurationBuilder.AddEnvironmentVariables();

            this.configuration = configurationBuilder.Build();
        });


        builder.ConfigureTestServices(services => {

            //services.AddSingleton<DatabaseInContainerFixture>();

            services.AddHttpClient<IStockService, StockService>(client =>
                client.BaseAddress = new Uri(this.configuration.GetValue<string>(nameof(ServiceApiBaseUrl)))
            );

            services.AddDbContext<IRepository, RepositoryContext>(o =>
                o.UseSqlServer("Server=localhost;Database=PaperTrading;Persist Security Info=False;User Id=sa;Password=1StrongPassword;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=300;")
                //o.UseSqlServer(DataBaseContainer.GetConnectionString())
            );

            services.AddTransient<SecurityOrchestratorService>();

        });
    }


    public async Task InitializeAsync() 
    {
        //await WebserviceContainer.StartAsync();
        //await DataBaseContainer.StartAsync(); 
        await Task.Delay(1500);
        
    }


    public async new Task DisposeAsync()
    {
        //await DataBaseContainer.StopAsync();
        //await WebserviceContainer.StopAsync();
        await Task.Delay(1000);
    }

    
    private static IEnumerable<KeyValuePair<string, string?>> GetAppSettings()
    {
        return new Dictionary<string, string?>(){
            { nameof(ServiceApiBaseUrl) , ServiceApiBaseUrl }
        };
    }



}