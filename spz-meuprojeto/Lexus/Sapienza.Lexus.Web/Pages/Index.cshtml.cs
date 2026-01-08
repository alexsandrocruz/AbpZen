using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.Lexus.Web.Pages
{
    public class IndexModel : LexusPageModel
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