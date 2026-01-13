using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finPlanoContasGrupos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finPlanoContasGrupos;

public interface IfinPlanoContasGruposAppService :
    ICrudAppService<
        finPlanoContasGruposDto,
        Guid,
        finPlanoContasGruposGetListInput,
        CreateUpdatefinPlanoContasGruposDto,
        CreateUpdatefinPlanoContasGruposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasGruposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
