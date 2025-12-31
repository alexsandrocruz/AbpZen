using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.TagHelpers;

[HtmlTargetElement("leptonx-theme-appearance", TagStructure = TagStructure.NormalOrSelfClosing)]
public class LeptonXThemeAppearanceTagHelper : AbpTagHelper, ITransientDependency
{
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;
    
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.Content.Clear();
       
        var options = ViewContext.HttpContext.RequestServices.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;
        var leptonXStyleProvider = ViewContext.HttpContext.RequestServices.GetRequiredService<LeptonXStyleProvider>();
        var urlHelper = ViewContext.GetUrlHelper();
        
        var layout = options.Layout == LeptonXBlazorLayouts.SideMenu ? "side-menu" : "top-menu";
        var selectedStyle = await leptonXStyleProvider.GetSelectedStyleAsync();
        var selectedStyleFileName = CultureHelper.IsRtl ?  selectedStyle + ".rtl" :  selectedStyle;
        var rtl = CultureHelper.IsRtl ? ".rtl" : string.Empty;

        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"bootstrap-{selectedStyleFileName}.css", $"lpx-theme-bootstrap-{selectedStyle}"));
        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"{selectedStyleFileName}.css", $"lpx-theme-color-{selectedStyle}"));

        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"layout-bundle{rtl}.css", $"lpx-layout-bundle-style"));
        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"abp-bundle{rtl}.css", $"lpx-abp-bundle-style"));
        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"blazor-bundle{rtl}.css", $"lpx-blazor-bundle-style"));
        output.Content.AppendHtml(GetLinkHtml(urlHelper,layout, $"font-bundle{rtl}.css", $"lpx-font-bundle-style"));
    }

    private string GetLinkHtml(IUrlHelper urlHelper, string layout, string fileName, string id)
    {
        var url = urlHelper.Content($"~/_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/{layout}/css/{fileName}");
        return $"<link href=\"{url}\" type=\"text/css\" rel=\"stylesheet\" id=\"{id}\"/>{Environment.NewLine}";
    }
}