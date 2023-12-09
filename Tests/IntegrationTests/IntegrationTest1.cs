using ApiClient.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Xunit;


namespace IntegrationTests;

public class IntegrationTest1
{
    private readonly DummyService _factory;

    public IntegrationTest1(DummyService factory)
    {
        _factory = factory;
    }





    
    [Fact]
    public void HappyPath()
    {

        //@todo: verify how to import and configure the dependecies injection (DI) 
        //https://stackoverflow.com/questions/67556254/how-to-inject-dependencies-into-iclassfixture-in-xunit
        //IClassFixture<WebApplication<Program>> a;
        var str = _factory;

        var securities = new List<string>();
        //var repo = new RepositoryContext()
        //new SecurityOrchestratorService()

        Assert.NotEmpty(securities);

    }





}


