using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Volo.Forms.Pages;

public class IndexModel : FormsPageModel
{
    public void OnGet()
    {

    }

    public async Task OnPostLoginAsync()
    {
        await HttpContext.ChallengeAsync("oidc");
    }
}
