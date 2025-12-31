using System;
using System.Collections.Generic;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[Serializable]
public class UpdatePageFeedbackSettingsInput
{
    public List<UpdatePageFeedbackSettingDto> Settings { get; set; }

    public UpdatePageFeedbackSettingsInput()
    {
        Settings = new List<UpdatePageFeedbackSettingDto>();
    }
    
    public UpdatePageFeedbackSettingsInput(List<UpdatePageFeedbackSettingDto> settings)
    {
        Settings = settings;
    }
}