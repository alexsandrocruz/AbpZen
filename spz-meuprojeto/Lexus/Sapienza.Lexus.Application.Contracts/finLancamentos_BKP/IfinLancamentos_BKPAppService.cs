using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finLancamentos_BKP.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finLancamentos_BKP;

public interface IfinLancamentos_BKPAppService :
    ICrudAppService<
        finLancamentos_BKPDto,
        Guid,
        finLancamentos_BKPGetListInput,
        CreateUpdatefinLancamentos_BKPDto,
        CreateUpdatefinLancamentos_BKPDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinLancamentos_BKPLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
