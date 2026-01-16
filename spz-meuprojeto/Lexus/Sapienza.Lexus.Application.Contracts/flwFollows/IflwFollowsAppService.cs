using System;
using System.Threading.Tasks;
using Sapienza.Lexus.flwFollows.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.flwFollows;

public interface IflwFollowsAppService :
    ICrudAppService<
        flwFollowsDto,
        Guid,
        flwFollowsGetListInput,
        CreateUpdateflwFollowsDto,
        CreateUpdateflwFollowsDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetflwFollowsLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
