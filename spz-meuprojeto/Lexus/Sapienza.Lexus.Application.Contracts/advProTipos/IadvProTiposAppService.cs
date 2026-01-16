using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProTipos;

public interface IadvProTiposAppService :
    ICrudAppService<
        advProTiposDto,
        Guid,
        advProTiposGetListInput,
        CreateUpdateadvProTiposDto,
        CreateUpdateadvProTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
