using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProfissionaisNaturezas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProfissionaisNaturezas;

public interface IadvProfissionaisNaturezasAppService :
    ICrudAppService<
        advProfissionaisNaturezasDto,
        Guid,
        advProfissionaisNaturezasGetListInput,
        CreateUpdateadvProfissionaisNaturezasDto,
        CreateUpdateadvProfissionaisNaturezasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProfissionaisNaturezasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
