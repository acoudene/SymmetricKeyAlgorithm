using CryptographyProvider;
using System.Security.Cryptography;
using System.Text;

namespace BlazorApp;

public class AzureKeyvaultService : IStatefulKeyGenerator
{
  public string GenerateKey(byte keySizeInBytes)
  {
    // TODO
    string serializedData = "Anthony";
    string hash = ComputeHash(serializedData, keySizeInBytes);
    return hash;
  }

  public static String ComputeHash(string value, byte keySizeInBytes)
  {
    StringBuilder Sb = new StringBuilder();

    using var hash = SHA256.Create();

    Encoding enc = Encoding.UTF8;
    byte[] result = hash.ComputeHash(enc.GetBytes(value), 0, keySizeInBytes);

    foreach (byte b in result)
      Sb.Append(b.ToString("x2"));

    return Sb.ToString(0, keySizeInBytes);
  }
}
