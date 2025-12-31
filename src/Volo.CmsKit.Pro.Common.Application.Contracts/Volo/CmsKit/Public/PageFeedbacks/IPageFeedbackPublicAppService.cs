using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.PageFeedbacks;

public interface IPageFeedbackPublicAppService : IApplicationService
{
    Task<PageFeedbackDto> CreateAsync(CreatePageFeedbackInput input);

    Task<PageFeedbackDto> InitializeUserNoteAsync(InitializeUserNoteInput input);

    Task<PageFeedbackDto> ChangeIsUsefulAsync(ChangeIsUsefulInput input);
}