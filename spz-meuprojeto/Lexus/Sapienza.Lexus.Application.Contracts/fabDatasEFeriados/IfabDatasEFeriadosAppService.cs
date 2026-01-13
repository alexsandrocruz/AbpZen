using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabDatasEFeriados.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabDatasEFeriados;

public interface IfabDatasEFeriadosAppService :
    ICrudAppService<
        fabDatasEFeriadosDto,
        Guid,
        fabDatasEFeriadosGetListInput,
        CreateUpdatefabDatasEFeriadosDto,
        CreateUpdatefabDatasEFeriadosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabDatasEFeriadosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
