using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliGrupos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliGrupos;

public interface IadvCliGruposAppService :
    ICrudAppService<
        advCliGruposDto,
        Guid,
        advCliGruposGetListInput,
        CreateUpdateadvCliGruposDto,
        CreateUpdateadvCliGruposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliGruposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
