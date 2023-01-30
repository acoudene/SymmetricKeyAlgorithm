namespace CryptographyProvider;

/// <summary>
/// Provider of encryption
/// </summary>
public interface IStatefulCryptographyProvider
{
  /// <summary>
  /// Encrypt a clear string
  /// </summary>
  /// <param name="clearString"></param>
  /// <returns></returns>
  string Encrypt(string clearString);

  /// <summary>
  /// Decrypt an encrypted string
  /// </summary>
  /// <param name="encryptedString"></param>
  /// <returns></returns>
  string Decrypt(string encryptedString);
}