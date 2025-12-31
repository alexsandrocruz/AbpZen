using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling
{
    [DependsOn(
        typeof(SharedThemeGlobalScriptContributor)
    )]
    public class LeptonXGlobalScriptContributor : BundleContributor
    {
        private const string RootPath = "/Themes/LeptonX/Global";
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeMvcOptions>>().Value;

            if (options.ApplicationLayout == LeptonXMvcLayouts.SideMenu)
            {
                context.Files.Add($"{RootPath}/side-menu/js/lepton-x.bundle.min.js");
            }
            else if (options.ApplicationLayout == LeptonXMvcLayouts.TopMenu)
            {
                context.Files.Add($"{RootPath}/top-menu/js/lepton-x.bundle.min.js");
            }


            context.Files.AddIfNotContains($"{RootPath}/scripts/style-initializer.js");
        }
    }
}