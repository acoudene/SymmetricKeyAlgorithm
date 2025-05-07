using Azure.Security.KeyVault.Secrets;
using BlazorApp;
using BlazorApp.Client.Dtos;
using BlazorApp.Components;
using CryptographyProvider;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.AddMongoDBClient(connectionName: "mongodb");
builder.AddAzureKeyVaultClient(connectionName: "keyvault");

builder.Services.AddTransient(
    sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<CryptoDataDto>("CryptoData"));

builder.Services.AddSingleton<ICryptographyProvider, AeSCryptographyProvider>();
builder.Services.AddSingleton<IStatefulKeyGenerator, AzureKeyvaultService>();
builder.Services.AddSingleton<IStatefulCryptographyProvider, StatefulCryptographyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp.Client._Imports).Assembly);

app.MapControllers();

var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
ConventionRegistry.Register("IgnoreExtraElements", pack, t => true);

// TODO
// Prepare a secret
//var secretClient = app.Services.GetRequiredService<SecretClient>();
//const string secretName = "aes-secret-key";
//const string secretValue = "012345678901234567890123456789012";
//await secretClient.SetSecretAsync(secretName, secretValue);

app.Run();
