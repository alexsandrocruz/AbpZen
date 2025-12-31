using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Volo.Abp.OpenIddict.Pro.Pages;

public class IndexModel : AbpOpenIddictProPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
