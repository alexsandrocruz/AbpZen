using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProfissionaisEstados.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProfissionaisEstados;

public interface IadvProfissionaisEstadosAppService :
    ICrudAppService<
        advProfissionaisEstadosDto,
        Guid,
        advProfissionaisEstadosGetListInput,
        CreateUpdateadvProfissionaisEstadosDto,
        CreateUpdateadvProfissionaisEstadosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisEstadosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
