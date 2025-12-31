using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Views.Error.DefaultErrorComponent;

public class LeptonXErrorViewComponent : LeptonXViewComponentBase
{
	public IViewComponentResult Invoke(AbpErrorViewModel model, string defaultErrorMessageKey)
	{
		var leptonModel = new LeptonXErrorPageModel
		{
			ErrorInfo = model.ErrorInfo,
			HttpStatusCode = model.HttpStatusCode,
			DefaultErrorMessageKey = defaultErrorMessageKey
		};

		return View("~/Views/Error/DefaultErrorComponent/Default.cshtml", leptonModel);
	}
}
