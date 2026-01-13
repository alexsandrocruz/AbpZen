using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.Event.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.Event;

public interface IEventAppService :
    ICrudAppService<
        EventDto,
        Guid,
        EventGetListInput,
        CreateUpdateEventDto,
        CreateUpdateEventDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetEventLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
