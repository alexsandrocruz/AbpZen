using System;
using System.Threading.Tasks;
using Sapienza.Lexus.opoTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.opoTipos;

public interface IopoTiposAppService :
    ICrudAppService<
        opoTiposDto,
        Guid,
        opoTiposGetListInput,
        CreateUpdateopoTiposDto,
        CreateUpdateopoTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetopoTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
