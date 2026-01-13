using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabConfig.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabConfig;

public interface IfabConfigAppService :
    ICrudAppService<
        fabConfigDto,
        Guid,
        fabConfigGetListInput,
        CreateUpdatefabConfigDto,
        CreateUpdatefabConfigDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabConfigLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
