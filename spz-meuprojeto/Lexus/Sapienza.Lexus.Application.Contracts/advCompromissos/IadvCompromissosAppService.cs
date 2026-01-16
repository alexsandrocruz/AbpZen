using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCompromissos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCompromissos;

public interface IadvCompromissosAppService :
    ICrudAppService<
        advCompromissosDto,
        Guid,
        advCompromissosGetListInput,
        CreateUpdateadvCompromissosDto,
        CreateUpdateadvCompromissosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCompromissosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
