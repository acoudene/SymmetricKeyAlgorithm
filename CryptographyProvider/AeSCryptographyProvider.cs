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
    Guard.IsEqualTo(key.Length, KeySizeInBytes);

    // Create a new Aes object to generate the key and IV
    using Aes aes = Aes.Create();
    aes.Key = Encoding.UTF8.GetBytes(key);

    // Security improvement: generate a new IV for each encryption
    aes.GenerateIV(); 
    byte[] iv = aes.IV;
    byte[] encryptedArray;

    // Encrypt the data
    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    using (MemoryStream memoryStream = new MemoryStream())
    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
      {
        streamWriter.Write(clearString);
      }
      encryptedArray = memoryStream.ToArray();
    }

    // Concatenate IV with encrypted data
    byte[] result = new byte[iv.Length + encryptedArray.Length];
    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
    Buffer.BlockCopy(encryptedArray, 0, result, iv.Length, encryptedArray.Length);

    string encryptedString = Convert.ToBase64String(result);
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
    Guard.IsEqualTo(key.Length, KeySizeInBytes);

    // Get IV and cipher text from the encrypted string
    byte[] buffer = Convert.FromBase64String(encryptedString);
    byte[] iv = new byte[BlockSizeInBytes];
    byte[] cipherText = new byte[buffer.Length - iv.Length];
    Buffer.BlockCopy(buffer, 0, iv, 0, iv.Length);
    Buffer.BlockCopy(buffer, iv.Length, cipherText, 0, cipherText.Length);

    // Create a new Aes object to decrypt the data
    using Aes aes = Aes.Create();
    aes.Key = Encoding.UTF8.GetBytes(key);
    aes.IV = iv;

    // Decrypt the data
    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    using MemoryStream memoryStream = new MemoryStream(cipherText);
    using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
    using StreamReader streamReader = new StreamReader((Stream)cryptoStream);

    string decryptedString = streamReader.ReadToEnd();
    return decryptedString;
  }
}
