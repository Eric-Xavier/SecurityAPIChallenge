
using ApiClient.Controllers;
using ApiClient.Interfaces;
using ApiClient.Models.Enums;
using ApiClient.Services;
using ApiClient.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests;

public class SecondTests : 
    IClassFixture<CustomWebApplicationFactory>,
    IAsyncLifetime
{
    readonly CustomWebApplicationFactory _factory;
    readonly RepositoryContext _context;
    readonly ISecurityService _business;

    /// <summary>
    /// Test cases so that it can touch the Real database(containerzed)
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="outputHelper"></param>
    /// <see href="https://blog.jetbrains.com/dotnet/2023/10/24/how-to-use-testcontainers-with-dotnet-unit-tests/"/>
    /// <see href="https://abstarreveld.medium.com/unit-testing-with-xunit-and-testcontainers-4a69cd22f888"/>
    public SecondTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        var scope = _factory.Services.CreateScope();
        

        _context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        _business = scope.ServiceProvider.GetRequiredService<SecurityOrchestratorService>();
        

    }

    public Task DisposeAsync() =>
        _factory.DisposeAsync();

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        Debug.WriteLine("InitializeAsync: " + _context.Database.GetConnectionString());

        _context.Database.EnsureCreated();
    }

    [Fact]
    public void SimpleEmptySelect()
    {
        var list = _context.Securities.ToList();
        Assert.Empty(list);
    }

    [Fact]
    public async Task AddingItem()
    {
        var _paperName = "FIRSTITEM001";
        await _context.Securities.AddAsync(new ApiClient.Models.SecurityModel { Price = 55.33m, ISINCode = _paperName });
        var rows = await _context.SaveChangesAsync();

        Assert.True(rows > 0);
        Assert.NotNull(_context.Securities.FirstOrDefault(x=>x.ISINCode == _paperName && x.Price > 0));
    }


    [Fact]
    public async Task HappyPathE2E()
    {
        //arrange
        var securities = new[] { "ABCDEFGHIJKL" }.ToList();
        
        //act
        var list = await _business.ExecuteAsync(securities).ToListAsync();

        //result
        Assert.NotNull(list.FirstOrDefault(x => securities.Contains(x.isin)));
        Assert.Equal(ErrorCodes.NoError.ToString(), list.FirstOrDefault()?.status);

    }

}
