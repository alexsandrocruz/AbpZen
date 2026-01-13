using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finPlanoContas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finPlanoContas;

public interface IfinPlanoContasAppService :
    ICrudAppService<
        finPlanoContasDto,
        Guid,
        finPlanoContasGetListInput,
        CreateUpdatefinPlanoContasDto,
        CreateUpdatefinPlanoContasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
