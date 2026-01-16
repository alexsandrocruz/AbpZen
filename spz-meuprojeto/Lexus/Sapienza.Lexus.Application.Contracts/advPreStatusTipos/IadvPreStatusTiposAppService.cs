using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreStatusTipos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreStatusTipos;

public interface IadvPreStatusTiposAppService :
    ICrudAppService<
        advPreStatusTiposDto,
        Guid,
        advPreStatusTiposGetListInput,
        CreateUpdateadvPreStatusTiposDto,
        CreateUpdateadvPreStatusTiposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreStatusTiposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
