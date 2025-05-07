var builder = DistributedApplication.CreateBuilder(args);

// Azure Keyvault
// https://learn.microsoft.com/en-us/dotnet/aspire/security/azure-security-key-vault-integration?tabs=dotnet-cli
var keyVault = builder.AddAzureKeyVault("keyvault");

// MongoDB
// https://learn.microsoft.com/en-us/dotnet/aspire/database/mongodb-integration?tabs=dotnet-cli
var mongo = builder.AddMongoDB("mongo")
                   .WithLifetime(ContainerLifetime.Persistent);
var mongodb = mongo.AddDatabase("mongodb");

//// Blazor
builder.AddProject<Projects.BlazorApp>("blazorapp")
  .WithReference(keyVault)
  .WithReference(mongodb)
  .WaitFor(mongodb)
  .WaitFor(keyVault);

// Exécuter l'application
builder.Build().Run();
