using FishingTournamentTracker.Web;
using FishingTournamentTracker.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var fishingApiBaseUrl = builder.Configuration["FishingTournamentTrackerApiBaseUrl"];
builder.Services.AddHttpClient<ITokenService, TokenService>(httpClient => httpClient.BaseAddress = new Uri(fishingApiBaseUrl!));
builder.Services.AddHttpClient<IAdminService, AdminService>(httpClient => httpClient.BaseAddress = new Uri(fishingApiBaseUrl!));
builder.Services.AddHttpClient<IUserService, UserService>(httpClient => httpClient.BaseAddress = new Uri(fishingApiBaseUrl!));
builder.Services.AddHttpClient<ITournamentService, TournamentService>(httpClient => httpClient.BaseAddress = new Uri(fishingApiBaseUrl!));
builder.Services.AddMemoryCache();

await builder.Build().RunAsync();
