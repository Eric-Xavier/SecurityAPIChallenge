using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace IntegrationTests;
public class ExternalDependencies
{
    public MsSqlContainer msSqlContainer{get; private set;}

    public ExternalDependencies()
    {

    }


    public async Task SetUp()
    {

        var mssqlBuilder = new MsSqlBuilder();
        mssqlBuilder.WithCleanUp(true);
        mssqlBuilder.WithEnvironment("ACCEPT_EULA", "Y");
        mssqlBuilder.WithEnvironment("MSSQL_SA_PASSWORD", "1StrongPassword");
        mssqlBuilder.WithEnvironment("MSSQL_PID", "Developer");
        mssqlBuilder.WithPortBinding(1433);
        
        msSqlContainer = mssqlBuilder.Build();



        /*// Create a new instance of a container.
        var container = new ContainerBuilder()
          .WithImage("testcontainers/helloworld:1.1.0")
          
          // Bind port 8080 of the container to a random port on the host.
          .WithPortBinding(8080, true)
          
          // Wait until the HTTP endpoint of the container is available.
          .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
          
          // Build the container configuration.
          .Build();

        // Start the container.
        await container.StartAsync()
          .ConfigureAwait(false);

        // Create a new instance of HttpClient to send HTTP requests.
        var httpClient = new HttpClient();

        // Construct the request URI by specifying the scheme, hostname, assigned random host port, and the endpoint "uuid".
        var requestUri = new UriBuilder(Uri.UriSchemeHttp, container.Hostname, container.GetMappedPublicPort(8080), "uuid").Uri;

        // Send an HTTP GET request to the specified URI and retrieve the response as a string.
        var guid = await httpClient.GetStringAsync(requestUri)
          .ConfigureAwait(false);*/

    }


}
