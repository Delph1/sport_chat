using Microsoft.EntityFrameworkCore;
using Laktaren.Application.Interfaces;
using Laktaren.Infrastructure.Data;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

var neo4jUri = builder.Configuration["Neo4j:Uri"];
var neo4jUsername = builder.Configuration["Neo4j:Username"];
var neo4jPassword = builder.Configuration["Neo4j:Password"];

builder.Services.AddSingleton<IDriver>(GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUsername, neo4jPassword)));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
