using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreStatus.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreStatus;

public interface IadvPreStatusAppService :
    ICrudAppService<
        advPreStatusDto,
        Guid,
        advPreStatusGetListInput,
        CreateUpdateadvPreStatusDto,
        CreateUpdateadvPreStatusDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreStatusLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
