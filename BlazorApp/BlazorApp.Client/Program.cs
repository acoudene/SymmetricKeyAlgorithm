using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient(
  "Crypto", 
  client => client.BaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/crypto/"));

await builder.Build().RunAsync();
