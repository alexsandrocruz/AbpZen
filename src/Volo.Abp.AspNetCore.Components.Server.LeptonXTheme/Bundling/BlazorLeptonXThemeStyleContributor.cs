using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.FlagIconCss;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling
{
    [DependsOn(typeof(FlagIconCssStyleContributor))]
    public class BlazorLeptonXThemeStyleContributor : BundleContributor
    {
        private const string RootPath = "/_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme";
        public override Task ConfigureBundleAsync(BundleConfigurationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;

            // TODO: Move common files to a common folder.
            context.Files.AddIfNotContains($"{RootPath}/side-menu/libs/bootstrap-datepicker/css/bootstrap-datepicker.min.css");
            context.Files.AddIfNotContains($"{RootPath}/side-menu/libs/bootstrap-icons/font/bootstrap-icons.css");
            
            var rtl = CultureHelper.IsRtl ? ".rtl" : string.Empty;
            var layout = options.Layout == LeptonXBlazorLayouts.SideMenu ? "side-menu" : "top-menu";

            return Task.CompletedTask;
        }
    }
}