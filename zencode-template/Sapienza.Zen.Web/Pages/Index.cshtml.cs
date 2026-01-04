using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.Zen.Web.Pages
{
    public class IndexModel : SapienzaZenPageModel
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