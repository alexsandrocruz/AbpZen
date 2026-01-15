using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SitePageVersion.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SitePageVersion;

public interface ISitePageVersionAppService :
    ICrudAppService<
        SitePageVersionDto,
        Guid,
        SitePageVersionGetListInput,
        CreateUpdateSitePageVersionDto,
        CreateUpdateSitePageVersionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSitePageVersionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
