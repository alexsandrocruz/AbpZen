using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Admin.PageFeedbacks;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Components.CmsKitProSettingGroup.PageFeedbackSettings;

public class CmsKitProPageFeedbackSettingGroupViewComponent : AbpViewComponent
{
    protected IPageFeedbackSettingsAppService PageFeedbackSettingsAppService { get; }

    public CmsKitProPageFeedbackSettingGroupViewComponent(IPageFeedbackSettingsAppService pageFeedbackSettingsAppService)
    {
        PageFeedbackSettingsAppService = pageFeedbackSettingsAppService;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await PageFeedbackSettingsAppService.GetAsync();
        return View("~/Pages/CmsKit/Components/CmsKitProSettingGroup/PageFeedbackSettings/Default.cshtml", model);
    }
}
