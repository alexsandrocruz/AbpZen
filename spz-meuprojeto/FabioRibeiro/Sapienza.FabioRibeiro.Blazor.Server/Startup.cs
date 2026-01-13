using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Sapienza.FabioRibeiro.Blazor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<FabioRibeiroBlazorModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeApplication();
        }
    }
}
