using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPautaObs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPautaObs;

public interface IadvPautaObsAppService :
    ICrudAppService<
        advPautaObsDto,
        Guid,
        advPautaObsGetListInput,
        CreateUpdateadvPautaObsDto,
        CreateUpdateadvPautaObsDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPautaObsLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
