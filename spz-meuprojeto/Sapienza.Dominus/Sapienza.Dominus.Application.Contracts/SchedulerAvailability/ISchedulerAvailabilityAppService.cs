using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SchedulerAvailability.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SchedulerAvailability;

public interface ISchedulerAvailabilityAppService :
    ICrudAppService<
        SchedulerAvailabilityDto,
        Guid,
        SchedulerAvailabilityGetListInput,
        CreateUpdateSchedulerAvailabilityDto,
        CreateUpdateSchedulerAvailabilityDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSchedulerAvailabilityLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
