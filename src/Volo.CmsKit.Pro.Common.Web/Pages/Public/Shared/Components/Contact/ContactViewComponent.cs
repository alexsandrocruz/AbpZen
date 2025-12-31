using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Contact;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Contact;

[Widget(
    ScriptTypes = new[] { typeof(ContactWidgetScriptContributor) },
    RefreshUrl = "/CmsKitProPublicWidgets/Contact",
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsContact")]
public class ContactViewComponent : AbpViewComponent
{
    protected IContactPublicAppService ContactPublicAppService;

    public ContactViewComponent(IContactPublicAppService contactPublicAppService)
    {
        ContactPublicAppService = contactPublicAppService;
    }

    public virtual IViewComponentResult Invoke(string contactName)
    {
        var viewModel = new ContactViewModel()
        {
            ContactName = contactName
        };

        return View("~/Pages/Public/Shared/Components/Contact/Default.cshtml", viewModel);
    }
}

public class ContactViewModel
{
    public string ContactName { get; set; }

    [Required]
    [Display(Name = "Name")]
    [Placeholder("YourFullName")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Subject")]
    [Placeholder("SubjectPlaceholder")]
    public string Subject { get; set; }

    [Required]
    [Display(Name = "EmailAddress")]
    [Placeholder("YourEmailAddress")]
    public string EmailAddress { get; set; }

    [Required]
    [Display(Name = "YourMessage")]
    [Placeholder("HowMayWeHelpYou")]
    [TextArea(Rows = 3)]
    public string Message { get; set; }

    [HiddenInput]
    public string RecaptchaToken { get; set; }
}