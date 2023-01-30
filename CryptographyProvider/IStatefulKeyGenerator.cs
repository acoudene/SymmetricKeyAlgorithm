namespace CryptographyProvider
{
  public interface IStatefulKeyGenerator
  {
    string GenerateKey(byte keySizeInBytes);
  }
}
