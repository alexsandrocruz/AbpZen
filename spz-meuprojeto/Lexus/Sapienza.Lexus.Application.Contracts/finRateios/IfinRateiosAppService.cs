using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finRateios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finRateios;

public interface IfinRateiosAppService :
    ICrudAppService<
        finRateiosDto,
        Guid,
        finRateiosGetListInput,
        CreateUpdatefinRateiosDto,
        CreateUpdatefinRateiosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinRateiosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
