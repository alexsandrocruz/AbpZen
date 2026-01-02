using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace LeptonXDemoApp.MauiBlazor;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureContainer(new AbpAutofacServiceProviderFactory(new Autofac.ContainerBuilder()));
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        ConfigureConfiguration(builder);

        builder.Services.AddApplication<LeptonXDemoAppMauiBlazorModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
        });

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        app.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(app.Services);

        return app;
    }

    private static void ConfigureConfiguration(MauiAppBuilder builder)
    {
        var assembly = typeof(App).GetTypeInfo().Assembly;
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
        builder.Configuration.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.secrets.json", optional: true, false);
    }
}
