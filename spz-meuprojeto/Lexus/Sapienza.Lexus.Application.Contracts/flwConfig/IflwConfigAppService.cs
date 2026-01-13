using System;
using System.Threading.Tasks;
using Sapienza.Lexus.flwConfig.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.flwConfig;

public interface IflwConfigAppService :
    ICrudAppService<
        flwConfigDto,
        Guid,
        flwConfigGetListInput,
        CreateUpdateflwConfigDto,
        CreateUpdateflwConfigDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetflwConfigLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
