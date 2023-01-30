namespace CryptographyProvider;

/// <summary>
/// Provider of encryption
/// </summary>
public interface ICryptographyProvider
{
  byte KeySizeInBytes { get; }

  /// <summary>
  /// Encrypt a clear string
  /// </summary>
  /// <param name="clearString"></param>
  /// <param name="key"></param>
  /// <returns></returns>
  string Encrypt(string clearString, string key);

  /// <summary>
  /// Decrypt an encrypted string
  /// </summary>
  /// <param name="encryptedString"></param>
  /// <param name="key"></param>
  /// <returns></returns>
  string Decrypt(string encryptedString, string key);
}