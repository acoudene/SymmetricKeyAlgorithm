using System.Security.Cryptography;
using System.Text;

namespace CryptographyProvider;

public class HashHelper
{
  public static string ComputeHash(string value, byte keySizeInBytes)
  {
    if (value.Length < keySizeInBytes)
      throw new ArgumentException($"The value must be at least {keySizeInBytes} characters long.");

    StringBuilder Sb = new StringBuilder();

    using var hash = SHA256.Create();

    Encoding enc = Encoding.UTF8;
    byte[] result = hash.ComputeHash(enc.GetBytes(value), 0, keySizeInBytes);

    foreach (byte b in result)
      Sb.Append(b.ToString("x2"));

    return Sb.ToString(0, keySizeInBytes);
  }
}
