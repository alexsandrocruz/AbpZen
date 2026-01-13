using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessos;

public interface IadvProcessosAppService :
    ICrudAppService<
        advProcessosDto,
        Guid,
        advProcessosGetListInput,
        CreateUpdateadvProcessosDto,
        CreateUpdateadvProcessosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
