using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessosMeritos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessosMeritos;

public interface IadvProcessosMeritosAppService :
    ICrudAppService<
        advProcessosMeritosDto,
        Guid,
        advProcessosMeritosGetListInput,
        CreateUpdateadvProcessosMeritosDto,
        CreateUpdateadvProcessosMeritosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosMeritosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
