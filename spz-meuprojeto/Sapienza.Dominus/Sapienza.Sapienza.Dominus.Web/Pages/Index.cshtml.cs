using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.Sapienza.Dominus.Web.Pages
{
    public class IndexModel : Sapienza.DominusPageModel
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