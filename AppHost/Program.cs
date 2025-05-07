using Azure.Identity;
using Azure.Provisioning.KeyVault;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System;

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
