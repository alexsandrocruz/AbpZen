using System;
using System.Threading.Tasks;
using Sapienza.EventoZen.EventCommission.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen.EventCommission;

public interface IEventCommissionAppService :
    ICrudAppService<
        EventCommissionDto,
        Guid,
        EventCommissionGetListInput,
        CreateUpdateEventCommissionDto,
        CreateUpdateEventCommissionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetEventCommissionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
