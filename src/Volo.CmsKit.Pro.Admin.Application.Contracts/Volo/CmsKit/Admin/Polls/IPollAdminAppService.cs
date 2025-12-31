using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Polls;

public interface IPollAdminAppService
    : ICrudAppService<
        PollWithDetailsDto,
        PollDto,
        Guid,
        GetPollListInput,
        CreatePollDto,
        UpdatePollDto
    >
{
    Task<ListResultDto<PollWidgetDto>> GetWidgetsAsync();
    Task<GetResultDto> GetResultAsync(Guid id);
}