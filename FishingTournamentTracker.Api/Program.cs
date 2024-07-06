using FishingTournamentTracker.Api.Configuration;
using FishingTournamentTracker.Api.Repositories;
using FishingTournamentTracker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITournamentService, TournamentService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<ITournamentRepository, TournamentRepository>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<IFileParser, FileParser>();
builder.Services.AddOptions<DatabaseConfiguration>()
    .Configure(config => config.ConnectionString = builder.Configuration.GetConnectionString("FishingTournamentDbConnectionString")
    ?? throw new ArgumentException("could not find FishingTournamentDbConnectionString"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
