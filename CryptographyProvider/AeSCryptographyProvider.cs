using CommunityToolkit.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyProvider;

/// <summary>
/// Provider dedicated to AES encryption
/// </summary>
public class AeSCryptographyProvider : ICryptographyProvider
{
  public const byte BlockSizeInBytes = 16;

  public byte KeySizeInBytes { get; } = 32;

  /// <summary>
  /// String encryption
  /// </summary>
  /// <param name="clearString">string to encrypt</param>
  /// <param name="key"></param>
  /// <returns>Encrypted string</returns>  
  public string Encrypt(string clearString, string key)
  {
    if (string.IsNullOrWhiteSpace(clearString))
      return string.Empty;

    Guard.IsNotNullOrWhiteSpace(key);
 
    using Aes aes = Aes.Create();
 
    byte[] iv = new byte[BlockSizeInBytes];
    byte[] array;

    aes.Key = Encoding.UTF8.GetBytes(key);
    aes.IV = iv;

    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

    using (MemoryStream memoryStream = new MemoryStream())
    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
      {
        streamWriter.Write(clearString);
      }
      array = memoryStream.ToArray();
    }

    string encryptedString = Convert.ToBase64String(array);
    return encryptedString;

  }

  /// <summary>
  /// String decryption
  /// </summary>
  /// <param name="encryptedString">string to decrypt</param>
  /// <param name="key"></param>
  /// <returns>Decrypted string</returns>
  public string Decrypt(string encryptedString, string key)
  {
    if (string.IsNullOrWhiteSpace(encryptedString))
      return string.Empty;

    Guard.IsNotNullOrWhiteSpace(key);

    using Aes aes = Aes.Create();
    
    byte[] iv = new byte[BlockSizeInBytes];
    byte[] buffer = Convert.FromBase64String(encryptedString);

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
