using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finExtrato.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finExtrato;

public interface IfinExtratoAppService :
    ICrudAppService<
        finExtratoDto,
        Guid,
        finExtratoGetListInput,
        CreateUpdatefinExtratoDto,
        CreateUpdatefinExtratoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinExtratoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
