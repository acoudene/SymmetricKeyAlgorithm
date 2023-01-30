using CommunityToolkit.Diagnostics;

namespace CryptographyProvider;

public class StatefulCryptographyProvider : IStatefulCryptographyProvider
{
  private readonly ICryptographyProvider _cryptographyProvider;
  private readonly IStatefulKeyGenerator _keyGenerator;

  public StatefulCryptographyProvider(ICryptographyProvider cryptographyProvider, IStatefulKeyGenerator keyGenerator)
  {
    Guard.IsNotNull(cryptographyProvider);
    Guard.IsNotNull(keyGenerator);

    _cryptographyProvider= cryptographyProvider;
    _keyGenerator= keyGenerator;
  }

  public string Decrypt(string encryptedString)
  {
    byte keySize = _cryptographyProvider.KeySizeInBytes;
    string key = _keyGenerator.GenerateKey(keySize);
    return _cryptographyProvider.Decrypt(encryptedString, key);
  }

  public string Encrypt(string clearString)
  {
    byte keySize = _cryptographyProvider.KeySizeInBytes;
    string key = _keyGenerator.GenerateKey(keySize);
    return _cryptographyProvider.Encrypt(clearString, key);
  }
}
