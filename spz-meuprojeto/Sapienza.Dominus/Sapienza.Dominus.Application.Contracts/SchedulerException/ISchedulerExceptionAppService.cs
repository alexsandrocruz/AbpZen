using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SchedulerException.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SchedulerException;

public interface ISchedulerExceptionAppService :
    ICrudAppService<
        SchedulerExceptionDto,
        Guid,
        SchedulerExceptionGetListInput,
        CreateUpdateSchedulerExceptionDto,
        CreateUpdateSchedulerExceptionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSchedulerExceptionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
