using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

// services
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITournamentService, TournamentService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<ITournamentRepository, TournamentRepository>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<IFileParser, FileParser>();

// configurations
builder.Services.AddOptions<DatabaseConfiguration>()
    .Configure(config => config.ConnectionString = builder.Configuration.GetConnectionString("FishingTournamentDbConnectionString")
    ?? throw new ArgumentException("could not find FishingTournamentDbConnectionString"));
builder.Services.AddOptions<TokenConfiguration>().Bind(builder.Configuration.GetSection("TokenConfiguration"));

// authorization for controllers
var audience = builder.Configuration["TokenConfiguration:Audience"];
var issuer = builder.Configuration["TokenConfiguration:Issuer"];
var key = builder.Configuration["TokenConfiguration:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
