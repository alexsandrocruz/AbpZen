using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finPlanoContasDet.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finPlanoContasDet;

public interface IfinPlanoContasDetAppService :
    ICrudAppService<
        finPlanoContasDetDto,
        Guid,
        finPlanoContasDetGetListInput,
        CreateUpdatefinPlanoContasDetDto,
        CreateUpdatefinPlanoContasDetDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasDetLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
