using System;
using System.Threading.Tasks;
using Sapienza.Lexus.opoOrcamentos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.opoOrcamentos;

public interface IopoOrcamentosAppService :
    ICrudAppService<
        opoOrcamentosDto,
        Guid,
        opoOrcamentosGetListInput,
        CreateUpdateopoOrcamentosDto,
        CreateUpdateopoOrcamentosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetopoOrcamentosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
