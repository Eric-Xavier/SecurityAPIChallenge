using BNPISINClient.Interfaces;
using BNPISINClient.Services;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IRepository, RepositoryService>();
builder.Services.AddTransient<ISecurityService, SecurityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
