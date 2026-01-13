using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliCargos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliCargos;

public interface IadvCliCargosAppService :
    ICrudAppService<
        advCliCargosDto,
        Guid,
        advCliCargosGetListInput,
        CreateUpdateadvCliCargosDto,
        CreateUpdateadvCliCargosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliCargosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
