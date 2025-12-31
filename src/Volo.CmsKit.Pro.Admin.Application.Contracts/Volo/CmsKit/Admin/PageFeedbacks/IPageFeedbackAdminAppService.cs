using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.PageFeedbacks;

public interface IPageFeedbackAdminAppService : IReadOnlyAppService<PageFeedbackDto, Guid, GetPageFeedbackListInput>, 
    IUpdateAppService<PageFeedbackDto, Guid, UpdatePageFeedbackDto>, IDeleteAppService<Guid>
{
    public Task<List<string>> GetEntityTypesAsync();
    
    public Task<List<PageFeedbackSettingDto>> GetSettingsAsync();

    public Task UpdateSettingsAsync(UpdatePageFeedbackSettingsInput input);
}