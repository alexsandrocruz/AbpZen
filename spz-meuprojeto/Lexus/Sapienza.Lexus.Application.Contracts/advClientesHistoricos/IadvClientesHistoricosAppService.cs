using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesHistoricos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesHistoricos;

public interface IadvClientesHistoricosAppService :
    ICrudAppService<
        advClientesHistoricosDto,
        Guid,
        advClientesHistoricosGetListInput,
        CreateUpdateadvClientesHistoricosDto,
        CreateUpdateadvClientesHistoricosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesHistoricosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
