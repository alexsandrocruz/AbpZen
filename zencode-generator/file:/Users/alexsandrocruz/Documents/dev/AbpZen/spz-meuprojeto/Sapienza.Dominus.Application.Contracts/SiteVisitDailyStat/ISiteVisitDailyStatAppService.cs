using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SiteVisitDailyStat.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SiteVisitDailyStat;

public interface ISiteVisitDailyStatAppService :
    ICrudAppService<
        SiteVisitDailyStatDto,
        Guid,
        SiteVisitDailyStatGetListInput,
        CreateUpdateSiteVisitDailyStatDto,
        CreateUpdateSiteVisitDailyStatDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSiteVisitDailyStatLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
