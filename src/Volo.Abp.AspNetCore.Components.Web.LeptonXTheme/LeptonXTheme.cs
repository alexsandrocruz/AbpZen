using System;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;

[ThemeName(Name)]
public class LeptonXTheme : ITheme, ITransientDependency
{
    public const string Name = "LeptonX";

    private readonly LeptonXThemeBlazorOptions _options;

    public LeptonXTheme(IOptions<LeptonXThemeBlazorOptions> options)
    {
        _options = options.Value;
    }

    public Type GetLayout(string name, bool fallbackToDefault = true)
    {
        switch (name)
        {
            case StandardLayouts.Application:
            case StandardLayouts.Account:
            case StandardLayouts.Public:
            case StandardLayouts.Empty:
                return _options.Layout;
            default:
                return fallbackToDefault ? _options.Layout : typeof(NullLayout);
        }
    }
}