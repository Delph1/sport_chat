using Microsoft.EntityFrameworkCore;
using Laktaren.Application.Interfaces;
using Laktaren.Infrastructure.Data;
using Neo4j.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

var neo4jUri = builder.Configuration["Neo4j:Uri"];
var neo4jUsername = builder.Configuration["Neo4j:Username"];
var neo4jPassword = builder.Configuration["Neo4j:Password"];

builder.Services.AddSingleton<IDriver>(GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUsername, neo4jPassword)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<ITeamRepository,  TeamRepository>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSvelteClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var key = Encoding.ASCII.GetBytes("DenHarNyckelnMasteVaraMinst32TeckenLangOchMycketHemlig!!!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // Sätt till true i produktion när du har en riktig domän
            ValidateAudience = false // Sätt till true i produktion
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowSvelteClient");

app.MapControllers();

app.Run();
