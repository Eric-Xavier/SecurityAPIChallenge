using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    public class DatabaseInContainerFixture
    {
        public MsSqlContainer msSqlContainer { get; private set; }
        const string MSSQLSAPASSWORD = "1StrongPassword";
        //readonly int _randomSqlPort = Random.Shared.Next(1400, 1499);


        public DatabaseInContainerFixture()
        {
            msSqlContainer = new MsSqlBuilder()
                .WithName("sut_sqlserver")
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithEnvironment("MSSQL_PID", "Developer")
                .WithPortBinding(MsSqlBuilder.MsSqlPort, MsSqlBuilder.MsSqlPort)
                .WithPassword(MSSQLSAPASSWORD)
                .WithCleanUp(true)
                .Build();

            msSqlContainer.Starting += MsSqlContainer_Starting;
            msSqlContainer.Started += MsSqlContainer_Started;
        }

        private void MsSqlContainer_Started(object? sender, EventArgs e)
        {
            Debug.WriteLine((sender as MsSqlContainer)?.Id, "#Container ID");
        }

        private void MsSqlContainer_Starting(object? sender, EventArgs e)
        {
            Debug.WriteLine((sender as MsSqlContainer)?.GetConnectionString(), "#Container Starting over");
        }

        /// <summary>
        /// Facade for <see cref="MsSqlContainer.StartAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task Start() => msSqlContainer.StartAsync();

        /// <summary>
        /// Facade for <see cref="MsSqlContainer.StopAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task Stop() => msSqlContainer.StopAsync();

    }





    public class SecuritiesDataProviderFixture
    {
        const int serverPort = 4000;
        public DockerContainer Container { get; init; }

        private string payload()
        {
            return JsonConvert.SerializeObject(
               new
               {
                   securityprice = new[] {
                       new { isin = "ABCDEFGHIJKL", price = 15.92m  },
                       new { isin = "US4581401001", price = 43.60m  },
                       new { isin = "XS0356705219", price = 67.95m  },
                       new { isin = "CH0031240127", price = 65.31m  },
                       new { isin = "CA9861913023", price = 13.75m  }
                   }
               });
        }


        /// <summary>
        /// @todo: it's not running yet
        /// </summary>
        public SecuritiesDataProviderFixture()
        {
            var container = new ContainerBuilder()
                .WithImage("node:latest")
                .WithName("dummyservice")
                .WithPortBinding(4000, 4000)
                .WithCommand("echo", $"'{payload()}'", ">" , "~/db.json", "&&")
                .WithCommand("npm", "install", "-g", "json-server", "--save-dev", "&&")
                .WithCommand("json-server", "--watch", "~/db.json", "--port", "{serverPort}", "--host", "0.0.0.0", "--id", "isin", "--read-only")
                //.WithEntrypoint("json-server", "--watch", "~/db.json", "--port", $"{serverPort}", "--host", "0.0.0.0", "--id", "isin", "--read-only")
                .WithCleanUp(true)
                .Build();

            Container = container as DockerContainer;
            //container.CopyAsync();
            
            

            Container.Creating += (sender, e) => Debug.WriteLine("Creating Json-Server");
            Container.Created += (sender, e) => Debug.WriteLine("Created Json-Server");
            Container.Starting += (sender, e) => Debug.WriteLine("Starting Json-Server");
            Container.Started += (sender, e) =>
            {
                var c = (DockerContainer)sender;
                var file = c.ReadFileAsync("~/db.json").GetAwaiter().GetResult();
                var content = UTF8Encoding.UTF8.GetString(file);


                Debug.WriteLine($"Created Json-Server, check \" docker logs {c.Id}");
                Debug.WriteLine(content);



            };
            Container.Stopped += (sender, e) =>
            {
                Debug.WriteLine("Stopped Json-Server");
            };
            Container.Stopping += (sender, e) => Debug.WriteLine("Stopping Json-Server");

        }


        public Task StartAsync(CancellationToken ct = default) => Container.StartAsync(ct);
        public Task StopAsync(CancellationToken ct = default) => Container.StopAsync(ct);
    }






}
