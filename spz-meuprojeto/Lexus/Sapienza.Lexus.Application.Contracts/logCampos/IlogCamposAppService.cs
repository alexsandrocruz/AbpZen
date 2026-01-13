using System;
using System.Threading.Tasks;
using Sapienza.Lexus.logCampos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.logCampos;

public interface IlogCamposAppService :
    ICrudAppService<
        logCamposDto,
        Guid,
        logCamposGetListInput,
        CreateUpdatelogCamposDto,
        CreateUpdatelogCamposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetlogCamposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
