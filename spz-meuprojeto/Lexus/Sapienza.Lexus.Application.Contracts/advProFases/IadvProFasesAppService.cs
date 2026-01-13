using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProFases.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProFases;

public interface IadvProFasesAppService :
    ICrudAppService<
        advProFasesDto,
        Guid,
        advProFasesGetListInput,
        CreateUpdateadvProFasesDto,
        CreateUpdateadvProFasesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProFasesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
