using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.LeptonX.Shared;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;

public class LeptonXStyleProvider : ITransientDependency
{
	private const string LEPTONX_STYLE_COOKIE_NAME = "lpx_loaded-css";

	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly LeptonXThemeOptions _leptonXThemeOption;

	public LeptonXStyleProvider(IHttpContextAccessor httpContextAccessor, IOptions<LeptonXThemeOptions> leptonXThemeOption)
	{
		_httpContextAccessor = httpContextAccessor;
		_leptonXThemeOption = leptonXThemeOption.Value;
	}

	public virtual Task<string> GetSelectedStyleAsync()
	{
		var styleName = _httpContextAccessor.HttpContext?.Request.Cookies[LEPTONX_STYLE_COOKIE_NAME];

		if (string.IsNullOrWhiteSpace(styleName) || !_leptonXThemeOption.Styles.ContainsKey(styleName))
		{
			if (_leptonXThemeOption.DefaultStyle == LeptonXStyleNames.System)
			{
				return Task.FromResult(LeptonXStyleNames.Dim);
			}

			return Task.FromResult(_leptonXThemeOption.DefaultStyle);
		}

		return Task.FromResult(styleName);
	}
}
