using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;

[ThemeName(Name)]
public class LeptonXTheme : ITheme, ITransientDependency
{
	public const string Name = "LeptonX";

	private readonly IConfiguration _configuration;
	private readonly LeptonXThemeMvcOptions _options;

	public LeptonXTheme(IConfiguration configuration, IOptions<LeptonXThemeMvcOptions> options)
	{
		_configuration = configuration;
		_options = options.Value;
	}

	public virtual string GetLayout(string name, bool fallbackToDefault = true)
	{
		switch (name)
		{
			case StandardLayouts.Application:
				return GetLayoutFromConfig("Application") ?? _options.ApplicationLayout;
			case StandardLayouts.Account:
				return GetLayoutFromConfig("Account") ?? "~/Themes/LeptonX/Layouts/Account/Default.cshtml";
			case StandardLayouts.Public:
				return GetLayoutFromConfig("Public") ?? _options.ApplicationLayout; // No-public layout yet
			case StandardLayouts.Empty:
				return GetLayoutFromConfig("Empty") ?? "~/Themes/LeptonX/Layouts/Empty/Default.cshtml";
			default:
				return fallbackToDefault ? "~/Themes/LeptonX/Layouts/Application/Default.cshtml" : null;
		}
	}

	private string GetLayoutFromConfig(string layoutName)
	{
		return _configuration["LeptonXTheme:Layouts:" + layoutName];
	}
}
