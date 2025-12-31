using System.Collections.Generic;
using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme
{
    public class LeptonXThemeBundleContributor : IBundleContributor
    {
        private const string RootPath = "_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme";
        private const string RootPathMauiBlazor = "_content/Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme";
        public void AddScripts(BundleContext context)
        {
            var layoutParameter = context.Parameters.GetValueOrDefault("LeptonXTheme.Layout", "side-menu");

            if (!context.InteractiveAuto)
            {
                context.Add($"{RootPath}/side-menu/libs/bootstrap/js/bootstrap.bundle.min.js");
            }

            context.Add($"{RootPath}/side-menu/libs/jquery/jquery.min.js");
            context.Add($"{RootPath}/side-menu/libs/bootstrap-datepicker/js/bootstrap-datepicker.min.js");
            context.Add($"{RootPath}/{layoutParameter}/js/lepton-x.bundle.min.js");

            context.Add($"{RootPath}/scripts/global.js");
            context.Add($"scripts/leptonx-mauiblazor-compatibility.js");
            context.BundleDefinitions.Insert(0,new BundleDefinition
            {
                Source = $"scripts/maui-blazor-patch.js"
            });
        }

        public void AddStyles(BundleContext context)
        {
            var layoutParameter = context.Parameters.GetValueOrDefault("LeptonXTheme.Layout", "side-menu");

            context.Add($"{RootPath}/side-menu/libs/bootstrap-datepicker/css/bootstrap-datepicker.min.css");
            context.Add($"{RootPath}/side-menu/libs/bootstrap-icons/font/bootstrap-icons.css");
        }
    }
}
