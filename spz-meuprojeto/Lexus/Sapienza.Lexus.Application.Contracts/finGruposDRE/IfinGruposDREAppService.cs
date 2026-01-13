using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finGruposDRE.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finGruposDRE;

public interface IfinGruposDREAppService :
    ICrudAppService<
        finGruposDREDto,
        Guid,
        finGruposDREGetListInput,
        CreateUpdatefinGruposDREDto,
        CreateUpdatefinGruposDREDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinGruposDRELookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
