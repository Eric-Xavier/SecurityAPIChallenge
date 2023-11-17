using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using System.Diagnostics;
using System.Reflection;
using Xunit.Sdk;

namespace IntegrationTests
{
    public class IntegratedTest1
    {
        

        

        [Fact(DisplayName = "DatabaseTest")]
        [BeforeTest]
        public async Task Test1()
        {
            Assert.True(false);

        }
    }


    class BeforeTest : BeforeAfterTestAttribute
    {
        const int dbport = 1433;

        public override async void Before(MethodInfo methodUnderTest)
        {

            var container = new ContainerBuilder()
                 .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                 .WithPortBinding(dbport, dbport)
                 .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(dbport))) // Wait until the HTTP endpoint of the container is available.                                              
                 .WithEnvironment("ACCEPT_EULA", "Y")
                 .WithEnvironment("MSSQL_SA_PASSWORD", "NotSoStrongPassword")
                 .WithEnvironment("MSSQL_PID", "Developer")
                 .WithEnvironment("TESTCONTAINERS_HOST_OVERRIDE", "host.docker.internal")
                 .WithVolumeMount("/var/run/docker.sock", "/var/run/docker.sock")
                 .Build();// Build the container configuration.


            // Start the container.
            await container.StartAsync()
              .ConfigureAwait(false);

        }
    }
}