using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advVerbas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advVerbas;

public interface IadvVerbasAppService :
    ICrudAppService<
        advVerbasDto,
        Guid,
        advVerbasGetListInput,
        CreateUpdateadvVerbasDto,
        CreateUpdateadvVerbasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvVerbasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
