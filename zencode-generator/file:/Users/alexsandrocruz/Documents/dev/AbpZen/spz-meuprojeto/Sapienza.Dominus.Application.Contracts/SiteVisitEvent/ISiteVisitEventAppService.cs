using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SiteVisitEvent.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SiteVisitEvent;

public interface ISiteVisitEventAppService :
    ICrudAppService<
        SiteVisitEventDto,
        Guid,
        SiteVisitEventGetListInput,
        CreateUpdateSiteVisitEventDto,
        CreateUpdateSiteVisitEventDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSiteVisitEventLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
