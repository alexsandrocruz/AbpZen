using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling
{
    public class BlazorLeptonXThemeScriptContributor : BundleContributor
    {
        private const string RootPath = "/_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme";
        private const string ServerRootPath = "/_content/Volo.Abp.AspNetCore.Components.Server.LeptonXTheme";
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;

            context.Files.AddIfNotContains($"{RootPath}/side-menu/libs/bootstrap/js/bootstrap.bundle.min.js");
            context.Files.AddIfNotContains($"{RootPath}/side-menu/libs/jquery/jquery.min.js");
            context.Files.AddIfNotContains($"{RootPath}/side-menu/libs/bootstrap-datepicker/js/bootstrap-datepicker.min.js");

            if (options.Layout == LeptonXBlazorLayouts.SideMenu)
            {
                context.Files.AddIfNotContains($"{RootPath}/side-menu/js/lepton-x.bundle.min.js");
            }
            else if (options.Layout == LeptonXBlazorLayouts.TopMenu)
            {
                context.Files.AddIfNotContains($"{RootPath}/top-menu/js/lepton-x.bundle.min.js");
            }

            context.Files.AddIfNotContains($"{ServerRootPath}/scripts/leptonx-blazor-compatibility.js");
            context.Files.AddIfNotContains($"{RootPath}/scripts/global.js");
        }
    }
}