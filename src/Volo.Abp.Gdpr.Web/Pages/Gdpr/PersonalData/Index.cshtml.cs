using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Gdpr;
using Volo.Abp.Gdpr.Web.Pages.Gdpr;

namespace Pages.Gdpr.PersonalData;

[Authorize]
public class Index : GdprPageModel
{
    public bool IsNewRequestAllowed { get; set; }
    
    protected IGdprRequestAppService GdprRequestAppService { get; }

    public Index(IGdprRequestAppService gdprRequestAppService)
    {
        GdprRequestAppService = gdprRequestAppService;
    }
    
    public async Task OnGetAsync()
    {
        IsNewRequestAllowed = await GdprRequestAppService.IsNewRequestAllowedAsync();
    }
}