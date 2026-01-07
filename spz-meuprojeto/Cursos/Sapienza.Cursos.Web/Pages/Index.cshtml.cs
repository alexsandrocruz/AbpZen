using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Sapienza.Cursos.Web.Pages
{
    public class IndexModel : CursosPageModel
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