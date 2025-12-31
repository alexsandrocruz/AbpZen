using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Forms.Permissions;

namespace Volo.Forms.Web.Pages.Forms;

[Authorize(FormsPermissions.Forms.Default)]
public class IndexModel : FormsPageModel
{
    public IndexModel()
    {
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
