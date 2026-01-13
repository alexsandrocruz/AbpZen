using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProVaras.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProVaras;

public interface IadvProVarasAppService :
    ICrudAppService<
        advProVarasDto,
        Guid,
        advProVarasGetListInput,
        CreateUpdateadvProVarasDto,
        CreateUpdateadvProVarasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProVarasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
