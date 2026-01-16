using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesChecklist.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesChecklist;

public interface IadvClientesChecklistAppService :
    ICrudAppService<
        advClientesChecklistDto,
        Guid,
        advClientesChecklistGetListInput,
        CreateUpdateadvClientesChecklistDto,
        CreateUpdateadvClientesChecklistDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesChecklistLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
