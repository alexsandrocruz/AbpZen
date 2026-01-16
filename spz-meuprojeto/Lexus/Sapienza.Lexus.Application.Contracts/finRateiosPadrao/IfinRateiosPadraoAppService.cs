using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finRateiosPadrao.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finRateiosPadrao;

public interface IfinRateiosPadraoAppService :
    ICrudAppService<
        finRateiosPadraoDto,
        Guid,
        finRateiosPadraoGetListInput,
        CreateUpdatefinRateiosPadraoDto,
        CreateUpdatefinRateiosPadraoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinRateiosPadraoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
