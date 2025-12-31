using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;

public class SettingsModal : AdminPageModel
{
    protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; }

    [BindProperty]
    public List<PageFeedbackSettingViewModel> Settings { get; set; } = new List<PageFeedbackSettingViewModel>();

    [BindProperty]
    public PageFeedbackSettingViewModel DefaultSetting { get; set; }

    public SettingsModal(IPageFeedbackAdminAppService pageFeedbackAdminAppService)
    {
        PageFeedbackAdminAppService = pageFeedbackAdminAppService;
    }


    public async Task OnGetAsync()
    {
        var entityTypes = await PageFeedbackAdminAppService.GetEntityTypesAsync();
        var settings = await PageFeedbackAdminAppService.GetSettingsAsync();
        var mappedSettings =
            ObjectMapper.Map<List<PageFeedbackSettingDto>, List<PageFeedbackSettingViewModel>>(settings);
        DefaultSetting = mappedSettings.FirstOrDefault(x => x.EntityType == null) ??
                               new PageFeedbackSettingViewModel();

        foreach (var entityType in entityTypes)
        {
            Settings.Add(mappedSettings.FirstOrDefault(x => x.EntityType == entityType) ??
                         new PageFeedbackSettingViewModel{ EntityType = entityType });
        }
    }

    public async Task OnPostAsync()
    {
        Settings.Add(DefaultSetting);
        var mappedSettings =
            ObjectMapper.Map<List<PageFeedbackSettingViewModel>, List<UpdatePageFeedbackSettingDto>>(Settings);
        await PageFeedbackAdminAppService.UpdateSettingsAsync(new UpdatePageFeedbackSettingsInput(mappedSettings));
    }

    public class PageFeedbackSettingViewModel
    {
        [HiddenInput] 
        public Guid Id { get; set; }

        [HiddenInput] 
        [ReadOnlyInput] 
        public string EntityType { get; set; }

        [DynamicMaxLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxEmailAddressesLength))]
        public string EmailAddresses { get; set; }
    }
}