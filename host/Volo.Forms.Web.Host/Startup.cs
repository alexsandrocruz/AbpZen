using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Forms;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<FormsWebHostModule>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.InitializeApplication();
    }
}
