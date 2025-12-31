using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Forms.Forms;
using Volo.Forms.Responses;

namespace Volo.Forms.Web.Pages.Forms.Shared.Components.ViewResponse;

[Widget(
    StyleFiles = new[] { "/Pages/Forms/Shared/Components/ViewResponse/Default.css" },
    ScriptTypes = new[] { typeof(ViewResponseWidgetScriptContributor) },
    AutoInitialize = true
)]
[ViewComponent(Name = "ViewResponse")]
public class ViewResponseViewComponent : AbpViewComponent
{
    protected IResponseAppService ResponseAppService { get; }
    public string ViewFormUrl { get; set; }

    public ViewResponseViewComponent(IResponseAppService responseAppService)
    {
        ResponseAppService = responseAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(Guid id)
    {
        var formResponse = await ResponseAppService.GetAsync(id);

        var form = await ResponseAppService.GetFormDetailsAsync(formResponse.FormId);

        var viewModel = new ViewResponseViewModel
        {
            Form = form,
            FormResponse = formResponse,
            ViewFormUrl = $"/Forms/{form.Id}/ViewForm"
        };

        return View("~/Pages/Forms/Shared/Components/ViewResponse/Default.cshtml", viewModel);
    }
}

public class ViewResponseViewModel
{
    public string ViewFormUrl { get; set; }
    public FormDto Form { get; set; }
    public FormResponseDto FormResponse { get; set; }
}
