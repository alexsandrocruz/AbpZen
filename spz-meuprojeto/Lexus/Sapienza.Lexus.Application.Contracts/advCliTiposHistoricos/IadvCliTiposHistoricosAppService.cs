using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliTiposHistoricos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliTiposHistoricos;

public interface IadvCliTiposHistoricosAppService :
    ICrudAppService<
        advCliTiposHistoricosDto,
        Guid,
        advCliTiposHistoricosGetListInput,
        CreateUpdateadvCliTiposHistoricosDto,
        CreateUpdateadvCliTiposHistoricosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliTiposHistoricosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
