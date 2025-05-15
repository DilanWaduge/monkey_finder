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

        builder.Services.TryAddTransient(serviceProvider => Connectivity.Current); // throws a compilation error
        builder.Services.TryAddSingleton(Geolocation.Default);
        builder.Services.TryAddTransient(serviceProvider => Map.Default);

        builder.Services.TryAddSingleton<MonkeyService>();

        builder.Services.TryAddSingleton<MonkeysViewModel>();
        builder.Services.TryAddTransient<MonkeyDetailsViewModel>();

        builder.Services.TryAddSingleton<MainPage>();
        builder.Services.TryAddTransient<DetailsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif



        return builder.Build();
    }
}