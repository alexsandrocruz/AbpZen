using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.CmsKit.Pro;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File("Logs/logs.txt")
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Host
#if MongoDB
                .ConfigureAppConfiguration(options =>
                {
                    options.AddJsonFile("appsettings.MongoDB.json");
                })
#endif
                .UseAutofac()
                .UseSerilog();
            
            await builder.AddApplicationAsync<ProWebUnifiedModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
