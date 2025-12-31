using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Admin.Contact;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Components.CmsKitProSettingGroup.ContactSettings;

public class CmsKitProContactSettingGroupViewComponent : AbpViewComponent
{
    protected IContactSettingsAppService ContactSettingsAppService { get; }

    public CmsKitProContactSettingGroupViewComponent(IContactSettingsAppService contactSettingsAppService)
    {
        ContactSettingsAppService = contactSettingsAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await ContactSettingsAppService.GetAsync();
        return View("~/Pages/CmsKit/Components/CmsKitProSettingGroup/ContactSettings/Default.cshtml", model);
    }
}
