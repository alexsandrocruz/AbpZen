using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreCheckLists.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreCheckLists;

public interface IadvPreCheckListsAppService :
    ICrudAppService<
        advPreCheckListsDto,
        Guid,
        advPreCheckListsGetListInput,
        CreateUpdateadvPreCheckListsDto,
        CreateUpdateadvPreCheckListsDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreCheckListsLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
