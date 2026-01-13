using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProInstancias.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProInstancias;

public interface IadvProInstanciasAppService :
    ICrudAppService<
        advProInstanciasDto,
        Guid,
        advProInstanciasGetListInput,
        CreateUpdateadvProInstanciasDto,
        CreateUpdateadvProInstanciasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProInstanciasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
