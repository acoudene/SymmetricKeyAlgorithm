/// Prerequesite: 
/// - when created here, azure keyvault should then have a secret named "aes-secret-key"
/// - to create a secret, user should have this role: KeyVaultSecretsOfficer
/// - These operations should be set before tested the first time.

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
  // TODO
  //.WithRoleAssignments(keyVault, KeyVaultBuiltInRole.KeyVaultSecretsOfficer)
  .WithReference(mongodb)
  .WaitFor(mongodb)
  .WaitFor(keyVault);

// Exécuter l'application
var app = builder.Build();
await app.RunAsync();
