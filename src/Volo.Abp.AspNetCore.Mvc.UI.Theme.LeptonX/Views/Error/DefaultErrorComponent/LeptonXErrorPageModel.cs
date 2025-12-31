using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Views.Error.DefaultErrorComponent;

public class LeptonXErrorPageModel : AbpErrorViewModel
{
	public string DefaultErrorMessageKey { get; set; }
}
