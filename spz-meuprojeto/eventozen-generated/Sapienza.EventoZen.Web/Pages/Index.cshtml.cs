using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.EventoZen.Web.Pages
{
    public class IndexModel : EventoZenPageModel
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