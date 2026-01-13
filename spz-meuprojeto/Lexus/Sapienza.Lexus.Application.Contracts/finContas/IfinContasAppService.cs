using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finContas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finContas;

public interface IfinContasAppService :
    ICrudAppService<
        finContasDto,
        Guid,
        finContasGetListInput,
        CreateUpdatefinContasDto,
        CreateUpdatefinContasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinContasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
