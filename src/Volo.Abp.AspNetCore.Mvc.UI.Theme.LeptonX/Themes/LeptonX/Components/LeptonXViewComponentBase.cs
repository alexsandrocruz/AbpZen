namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components;

public abstract class LeptonXViewComponentBase : AbpViewComponent
{
	protected LeptonXViewComponentBase()
	{
		ObjectMapperContext = typeof(AbpAspNetCoreMvcUiLeptonXThemeModule);
	}
}
