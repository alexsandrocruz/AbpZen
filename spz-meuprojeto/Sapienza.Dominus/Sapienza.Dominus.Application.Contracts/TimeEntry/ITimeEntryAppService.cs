using System;
using System.Threading.Tasks;
using Sapienza.Dominus.TimeEntry.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.TimeEntry;

public interface ITimeEntryAppService :
    ICrudAppService<
        TimeEntryDto,
        Guid,
        TimeEntryGetListInput,
        CreateUpdateTimeEntryDto,
        CreateUpdateTimeEntryDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetTimeEntryLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
