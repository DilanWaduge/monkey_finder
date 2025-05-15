using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.TryAddSingleton<MonkeyService>();
        builder.Services.TryAddSingleton<MonkeysViewModel>();

        builder.Services.TryAddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif



        return builder.Build();
    }
}