using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Site.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Site;

public interface ISiteAppService :
    ICrudAppService<
        SiteDto,
        Guid,
        SiteGetListInput,
        CreateUpdateSiteDto,
        CreateUpdateSiteDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSiteLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
