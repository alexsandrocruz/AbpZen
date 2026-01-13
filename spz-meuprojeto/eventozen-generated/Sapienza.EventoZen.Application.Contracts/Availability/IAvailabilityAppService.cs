using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.Availability.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.Availability;

public interface IAvailabilityAppService :
    ICrudAppService<
        AvailabilityDto,
        Guid,
        AvailabilityGetListInput,
        CreateUpdateAvailabilityDto,
        CreateUpdateAvailabilityDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetAvailabilityLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
