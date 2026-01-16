using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliBairros.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliBairros;

public interface IadvCliBairrosAppService :
    ICrudAppService<
        advCliBairrosDto,
        Guid,
        advCliBairrosGetListInput,
        CreateUpdateadvCliBairrosDto,
        CreateUpdateadvCliBairrosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliBairrosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
