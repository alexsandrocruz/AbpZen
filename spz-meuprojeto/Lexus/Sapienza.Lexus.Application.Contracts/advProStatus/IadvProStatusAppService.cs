using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProStatus.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProStatus;

public interface IadvProStatusAppService :
    ICrudAppService<
        advProStatusDto,
        Guid,
        advProStatusGetListInput,
        CreateUpdateadvProStatusDto,
        CreateUpdateadvProStatusDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProStatusLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
