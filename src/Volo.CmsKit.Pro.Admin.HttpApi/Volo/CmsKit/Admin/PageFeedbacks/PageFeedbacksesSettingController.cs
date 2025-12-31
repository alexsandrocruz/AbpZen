using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[RequiresGlobalFeature(typeof(PageFeedbackFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/page-feedbacks/settings")]
[Authorize(CmsKitProAdminPermissions.PageFeedbacks.SettingManagement)]
public class PageFeedbackSettingsController : CmsKitProAdminController, IPageFeedbackSettingsAppService
{
    protected IPageFeedbackSettingsAppService PageFeedbackSettingsAppService { get; }

    public PageFeedbackSettingsController(IPageFeedbackSettingsAppService pageFeedbackSettingsAppService)
    {
        PageFeedbackSettingsAppService = pageFeedbackSettingsAppService;
    }

    [HttpGet]
    public virtual Task<CmsKitPageFeedbackSettingDto> GetAsync()
    {
        return PageFeedbackSettingsAppService.GetAsync();
    }

    [HttpPost]
    public virtual Task UpdateAsync(UpdateCmsKitPageFeedbackSettingDto input)
    {
        return PageFeedbackSettingsAppService.UpdateAsync(input);
    }
}
