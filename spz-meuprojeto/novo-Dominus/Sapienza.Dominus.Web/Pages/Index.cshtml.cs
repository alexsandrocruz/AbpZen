using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.Dominus.Web.Pages
{
    public class IndexModel : DominusPageModel
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