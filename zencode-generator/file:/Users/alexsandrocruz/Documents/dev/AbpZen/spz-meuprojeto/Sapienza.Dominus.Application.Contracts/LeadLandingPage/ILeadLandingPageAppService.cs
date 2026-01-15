using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadLandingPage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadLandingPage;

public interface ILeadLandingPageAppService :
    ICrudAppService<
        LeadLandingPageDto,
        Guid,
        LeadLandingPageGetListInput,
        CreateUpdateLeadLandingPageDto,
        CreateUpdateLeadLandingPageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadLandingPageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
