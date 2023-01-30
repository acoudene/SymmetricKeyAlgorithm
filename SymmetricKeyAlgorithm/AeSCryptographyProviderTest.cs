using System.Text;

namespace SymmetricKeyAlgorithm;

public class AeSCryptographyProviderTest
{
  private readonly uint _longTextLimitValue = (uint)new Random().Next(59, 1025);
  private readonly uint _shortTextLimitValue = (uint)new Random().Next(1, 59);

  protected string GenerateRandomText(uint expectedLength)
  {
    char[] chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
    Random r = new Random();
    StringBuilder builder = new StringBuilder(Convert.ToInt32(expectedLength));
    for (int i = 0; i < expectedLength; i++)
    {
      int index = r.Next(chars.Length);
      builder.Append(chars[index]);
    }
    return builder.ToString();
  }

  [Fact]
  public void EncryptDecrypt_Ok()
  {
    // ARRANGE
    string myValue = GenerateRandomText(_longTextLimitValue);
    string key = GenerateRandomText(AeSCryptographyProvider.KeySizeInBytes);
    var provider = new AeSCryptographyProvider();
    
    // ACT
    string myValueEncrypted = provider.Encrypt(myValue, key);
    string myValueDecrypted = provider.Decrypt(myValueEncrypted, key);

    // ASSERT
    Assert.Equal(myValue, myValueDecrypted);

  }
}