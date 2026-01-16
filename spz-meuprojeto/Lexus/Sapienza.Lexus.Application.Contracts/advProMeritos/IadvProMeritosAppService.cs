using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProMeritos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProMeritos;

public interface IadvProMeritosAppService :
    ICrudAppService<
        advProMeritosDto,
        Guid,
        advProMeritosGetListInput,
        CreateUpdateadvProMeritosDto,
        CreateUpdateadvProMeritosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProMeritosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
