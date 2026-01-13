using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProRelevancias.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProRelevancias;

public interface IadvProRelevanciasAppService :
    ICrudAppService<
        advProRelevanciasDto,
        Guid,
        advProRelevanciasGetListInput,
        CreateUpdateadvProRelevanciasDto,
        CreateUpdateadvProRelevanciasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProRelevanciasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
