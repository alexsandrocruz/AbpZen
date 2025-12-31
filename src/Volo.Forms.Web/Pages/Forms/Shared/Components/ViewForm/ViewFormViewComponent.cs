using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.VeeValidate;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Vue;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Forms.Responses;

namespace Volo.Forms.Web.Pages.Forms.Shared.Components.ViewForm;

[Widget(
    StyleFiles = new[] {"/Pages/Forms/Shared/Components/ViewForm/Default.css"},
    ScriptTypes = new[] {
        typeof(VueScriptContributor), typeof(VeeValidateScriptContributor), typeof(ViewFormWidgetScriptContributor)
    },
    AutoInitialize = true
)]
[ViewComponent(Name = "ViewForm")]
public class ViewFormViewComponent : AbpViewComponent
{
    protected IResponseAppService ResponseAppService { get; }

    public ViewFormViewComponent(IResponseAppService responseAppService)
    {
        ResponseAppService = responseAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(Guid formId, bool preview = false)
    {
        var form = await ResponseAppService.GetFormDetailsAsync(formId);
        if (form == null)
        {
            return View("~/Pages/Forms/Shared/Components/ViewForm/NotFound.cshtml",
                new ViewFormViewModel {Id = formId, Preview = false});
        }

        return View("~/Pages/Forms/Shared/Components/ViewForm/Default.cshtml",
            new ViewFormViewModel {Id = form.Id, Preview = preview});
    }
}

public class ViewFormViewModel
{
    public Guid Id { get; set; }

    public bool Preview { get; set; } = false;
}