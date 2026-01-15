using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadStageHistory.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadStageHistory;

public interface ILeadStageHistoryAppService :
    ICrudAppService<
        LeadStageHistoryDto,
        Guid,
        LeadStageHistoryGetListInput,
        CreateUpdateLeadStageHistoryDto,
        CreateUpdateLeadStageHistoryDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadStageHistoryLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
