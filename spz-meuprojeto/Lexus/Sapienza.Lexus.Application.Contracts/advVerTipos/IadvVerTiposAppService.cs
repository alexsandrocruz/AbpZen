using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advVerTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advVerTipos;

public interface IadvVerTiposAppService :
    ICrudAppService<
        advVerTiposDto,
        Guid,
        advVerTiposGetListInput,
        CreateUpdateadvVerTiposDto,
        CreateUpdateadvVerTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvVerTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
