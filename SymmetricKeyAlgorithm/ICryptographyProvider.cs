namespace SymmetricKeyAlgorithm;

/// <summary>
/// Provider of encryption
/// </summary>
public interface ICryptographyProvider
{
  /// <summary>
  /// Encrypt a clear string
  /// </summary>
  /// <param name="clearString"></param>
  /// <param name="key"></param>
  /// <returns></returns>
  string EncryptString(string clearString, string key);

  /// <summary>
  /// Decrypt an encrypted string
  /// </summary>
  /// <param name="encryptedString"></param>
  /// <param name="key"></param>
  /// <returns></returns>
  string DecryptString(string encryptedString, string key);
}