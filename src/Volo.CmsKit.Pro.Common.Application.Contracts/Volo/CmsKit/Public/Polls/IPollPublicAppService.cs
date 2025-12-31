using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Polls;
public interface IPollPublicAppService : IApplicationService
{
    Task<bool> IsWidgetNameAvailableAsync(string widgetName);
    Task<PollWithDetailsDto> FindByAvailableWidgetAsync(string widgetName);
    Task<PollWithDetailsDto> FindByCodeAsync(string code);
    Task<GetResultDto> GetResultAsync(Guid id);
    Task SubmitVoteAsync(Guid id, SubmitPollInput submitPollInput);
}
