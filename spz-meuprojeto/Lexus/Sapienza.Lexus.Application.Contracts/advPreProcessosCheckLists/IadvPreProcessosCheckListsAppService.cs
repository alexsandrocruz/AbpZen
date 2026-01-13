using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

public interface IadvPreProcessosCheckListsAppService :
    ICrudAppService<
        advPreProcessosCheckListsDto,
        Guid,
        advPreProcessosCheckListsGetListInput,
        CreateUpdateadvPreProcessosCheckListsDto,
        CreateUpdateadvPreProcessosCheckListsDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreProcessosCheckListsLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
