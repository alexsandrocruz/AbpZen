using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace LeptonXDemoApp.Web.Pages
{
    public class IndexModel : LeptonXDemoAppPageModel
    {
        public void OnGet()
        {
            
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}