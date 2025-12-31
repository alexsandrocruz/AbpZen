using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.Common;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme
{
    public class LeptonXThemeWebToolbarContributor : IToolbarContributor
    {
        public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name == StandardToolbars.Main)
            {
                var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;

                context.Toolbar.Items.Add(new ToolbarItem(typeof(GeneralSettings)));
            }

            return Task.CompletedTask;
        }
    }
}
