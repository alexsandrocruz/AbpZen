using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SitePage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SitePage;

public interface ISitePageAppService :
    ICrudAppService<
        SitePageDto,
        Guid,
        SitePageGetListInput,
        CreateUpdateSitePageDto,
        CreateUpdateSitePageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSitePageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
