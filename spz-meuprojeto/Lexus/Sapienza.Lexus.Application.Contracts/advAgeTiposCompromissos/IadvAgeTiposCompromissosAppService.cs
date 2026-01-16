using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advAgeTiposCompromissos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advAgeTiposCompromissos;

public interface IadvAgeTiposCompromissosAppService :
    ICrudAppService<
        advAgeTiposCompromissosDto,
        Guid,
        advAgeTiposCompromissosGetListInput,
        CreateUpdateadvAgeTiposCompromissosDto,
        CreateUpdateadvAgeTiposCompromissosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvAgeTiposCompromissosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
