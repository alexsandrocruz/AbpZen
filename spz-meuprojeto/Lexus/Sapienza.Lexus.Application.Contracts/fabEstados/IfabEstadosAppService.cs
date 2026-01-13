using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabEstados.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabEstados;

public interface IfabEstadosAppService :
    ICrudAppService<
        fabEstadosDto,
        Guid,
        fabEstadosGetListInput,
        CreateUpdatefabEstadosDto,
        CreateUpdatefabEstadosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabEstadosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
