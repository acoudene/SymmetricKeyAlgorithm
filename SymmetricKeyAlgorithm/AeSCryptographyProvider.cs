
using CommunityToolkit.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace SymmetricKeyAlgorithm;


/// <summary>
/// Provider dedicated to AES encryption
/// </summary>
public class AeSCryptographyProvider : ICryptographyProvider
{
  /// <summary>
  /// String encryption
  /// </summary>
  /// <param name="clearString">string to encrypt</param>
  /// <param name="key"></param>
  /// <returns>Encrypted string</returns>  
  public string EncryptString(string clearString, string key)
  {
    if (string.IsNullOrWhiteSpace(clearString))
      return string.Empty;

    Guard.IsNotNullOrWhiteSpace(key);

    byte[] iv = new byte[16];
    byte[] array;

    using Aes aes = Aes.Create();

    aes.Key = Encoding.UTF8.GetBytes(key);
    aes.IV = iv;

    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

    using MemoryStream memoryStream = new MemoryStream();
    using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
    using StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream);

    streamWriter.Write(clearString);
    array = memoryStream.ToArray();

    string encryptedString = Convert.ToBase64String(array);
    return encryptedString;
  }

  /// <summary>
  /// String decryption
  /// </summary>
  /// <param name="encryptedString">string to decrypt</param>
  /// <param name="key"></param>
  /// <returns>Decrypted string</returns>
  public string DecryptString(string encryptedString, string key)
  {    
    if (string.IsNullOrWhiteSpace(encryptedString))
      return string.Empty;

    Guard.IsNotNullOrWhiteSpace(key);

    byte[] iv = new byte[16];
    byte[] buffer = Convert.FromBase64String(encryptedString);

    using Aes aes = Aes.Create();

    aes.Key = Encoding.UTF8.GetBytes(key);
    aes.IV = iv;
    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

    using MemoryStream memoryStream = new MemoryStream(buffer);
    using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
    using StreamReader streamReader = new StreamReader((Stream)cryptoStream);

    string decryptedString = streamReader.ReadToEnd();
    return decryptedString;
  }
}
