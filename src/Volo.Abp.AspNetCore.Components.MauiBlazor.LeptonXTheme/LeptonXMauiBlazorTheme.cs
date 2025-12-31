using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.AccountLayout;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme;

[ThemeName(Name)]
public class LeptonXMauiBlazorTheme : ITheme, ITransientDependency
{
    public const string Name = "LeptonXMauiBlazor";
    
    private readonly LeptonXThemeBlazorOptions _options;

    public LeptonXMauiBlazorTheme(IOptions<LeptonXThemeBlazorOptions> options)
    {
        _options = options.Value;
    }

    public virtual Type GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
            case StandardLayouts.Public:
            case StandardLayouts.Empty:
                return _options.Layout;
            case StandardLayouts.Account:
                return typeof(AccountLayout);
            default:
                return fallbackToDefault ? _options.Layout : typeof(NullLayout);
        }
    }
}