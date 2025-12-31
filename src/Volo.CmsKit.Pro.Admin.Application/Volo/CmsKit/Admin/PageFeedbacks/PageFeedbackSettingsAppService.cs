using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.SettingManagement;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[RequiresFeature(CmsKitProFeatures.PageFeedbackEnable)]
[RequiresGlobalFeature(PageFeedbackFeature.Name)]
[Authorize(CmsKitProAdminPermissions.PageFeedbacks.SettingManagement)]
public class PageFeedbackSettingsAppService : CmsKitProAdminAppService, IPageFeedbackSettingsAppService
{
    protected ISettingManager SettingManager { get; }

    public PageFeedbackSettingsAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    public virtual async Task<CmsKitPageFeedbackSettingDto> GetAsync()
    {
        var isAutoHandled = (await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.IsAutoHandled)).To<bool>();
        
        var requireCommentsForNegativeFeedback = (await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.RequireCommentsForNegativeFeedback)).To<bool>();

        return new CmsKitPageFeedbackSettingDto
        {
            IsAutoHandled = isAutoHandled,
            RequireCommentsForNegativeFeedback = requireCommentsForNegativeFeedback
        };
    }

    public virtual async Task UpdateAsync(UpdateCmsKitPageFeedbackSettingDto input)
    {
        await SettingManager.SetForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.IsAutoHandled, input.IsAutoHandled.ToString());
        await SettingManager.SetForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.RequireCommentsForNegativeFeedback, input.RequireCommentsForNegativeFeedback.ToString());
    }
}
