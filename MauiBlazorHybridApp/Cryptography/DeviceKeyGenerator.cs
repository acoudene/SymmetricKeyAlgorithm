using CryptographyProvider;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace MauiBlazorHybridApp.Cryptography;
public class DeviceKeyGenerator : IStatefulKeyGenerator
{
  public string GenerateKey(byte keySizeInBytes)
  {
    var currentDeviceInfo = DeviceInfo.Current;
    string serializedData = JsonConvert.SerializeObject(currentDeviceInfo);
    string hash = HashHelper.ComputeHash(serializedData, keySizeInBytes);
    return hash;
  }  
}
