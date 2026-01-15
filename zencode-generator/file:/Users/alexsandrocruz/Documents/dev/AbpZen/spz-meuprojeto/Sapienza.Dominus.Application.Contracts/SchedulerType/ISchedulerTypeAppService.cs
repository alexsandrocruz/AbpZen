using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SchedulerType.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SchedulerType;

public interface ISchedulerTypeAppService :
    ICrudAppService<
        SchedulerTypeDto,
        Guid,
        SchedulerTypeGetListInput,
        CreateUpdateSchedulerTypeDto,
        CreateUpdateSchedulerTypeDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSchedulerTypeLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
