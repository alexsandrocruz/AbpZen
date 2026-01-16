using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProNaturezas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProNaturezas;

public interface IadvProNaturezasAppService :
    ICrudAppService<
        advProNaturezasDto,
        Guid,
        advProNaturezasGetListInput,
        CreateUpdateadvProNaturezasDto,
        CreateUpdateadvProNaturezasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProNaturezasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
