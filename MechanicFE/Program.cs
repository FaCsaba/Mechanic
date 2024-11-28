using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MechanicFE;
using MechanicFE.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5052") });
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICommissionService, CommissionService>();

await builder.Build().RunAsync();