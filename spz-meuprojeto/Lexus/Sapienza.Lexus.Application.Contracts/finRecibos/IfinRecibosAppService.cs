using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finRecibos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finRecibos;

public interface IfinRecibosAppService :
    ICrudAppService<
        finRecibosDto,
        Guid,
        finRecibosGetListInput,
        CreateUpdatefinRecibosDto,
        CreateUpdatefinRecibosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinRecibosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
