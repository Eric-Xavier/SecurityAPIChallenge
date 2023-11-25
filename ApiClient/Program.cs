using ApiClient.Controllers;
using ApiClient.Interfaces;
using ApiClient.Services;
using ApiClient.Services.Stock;
using ApiClient.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IStockService, StockService>(client =>
{
     client.BaseAddress = new Uri(builder.Configuration[StockService.ServiceApiUrlConfigName]);
});

//builder.Services.AddTransient<IRepository, RepositoryService>();
builder.Services.AddTransient<IRepository, RepositoryContext>();
builder.Services.AddDbContext<RepositoryContext>(o=>{
    o.UseSqlServer(builder.Configuration.GetConnectionString(RepositoryContext.SqlServerConnectionPropertyName));
});
builder.Services.AddTransient<ISecurityService, SecurityOrchestratorService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Security API Challenge",
        Description = "An Web API that process data in a dummy service",
        Contact = new OpenApiContact { Name = "Eric Xavier" },
    });
    options.SchemaFilter<SwaggerCustomSchemaValues>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.EnableFilter();
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle= "Security Api Challenge";
    
});


app.Run();
