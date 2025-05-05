using CryptographyProvider;
using MauiBlazorHybridApp.Cryptography;
using Microsoft.Extensions.Logging;

namespace MauiBlazorHybridApp
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
          fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });

      builder.Services.AddMauiBlazorWebView();

#if DEBUG
  		builder.Services.AddBlazorWebViewDeveloperTools();
  		builder.Logging.AddDebug();
#endif
      
      builder.Services.AddSingleton<ICryptographyProvider, AeSCryptographyProvider>();
      builder.Services.AddSingleton<IStatefulKeyGenerator, DeviceKeyGenerator>();
      builder.Services.AddSingleton<IStatefulCryptographyProvider, StatefulCryptographyProvider>();

      return builder.Build();
    }
  }
}
