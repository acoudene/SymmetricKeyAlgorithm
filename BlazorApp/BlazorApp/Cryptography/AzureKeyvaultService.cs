using Azure.Security.KeyVault.Secrets;
using CryptographyProvider;

namespace BlazorApp;

public class AzureKeyvaultService : IStatefulKeyGenerator
{
  private readonly SecretClient _secretClient;
  
  public AzureKeyvaultService(SecretClient secretClient)
  {
    _secretClient = secretClient;
  }

  public string GenerateKey(byte keySizeInBytes)
  {
    // TODO
    string serializedData = _secretClient.GetSecret("aes-secret-key").Value.Value;
    string hash = HashHelper.ComputeHash(serializedData, keySizeInBytes);
    return hash;
  }
}
