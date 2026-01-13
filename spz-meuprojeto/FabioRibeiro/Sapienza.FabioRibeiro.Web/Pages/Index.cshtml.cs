using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.FabioRibeiro.Web.Pages
{
    public class IndexModel : FabioRibeiroPageModel
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