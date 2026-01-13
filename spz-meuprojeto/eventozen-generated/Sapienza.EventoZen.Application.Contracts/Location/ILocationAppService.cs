using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.Location.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.Location;

public interface ILocationAppService :
    ICrudAppService<
        LocationDto,
        Guid,
        LocationGetListInput,
        CreateUpdateLocationDto,
        CreateUpdateLocationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLocationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
