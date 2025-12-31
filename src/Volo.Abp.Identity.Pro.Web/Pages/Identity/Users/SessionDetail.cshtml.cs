using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users;

public class SessionDetailModel : IdentityPageModel
{
    public IdentitySessionDto Session { get; private set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    protected IIdentitySessionAppService IdentitySessionAppService { get; }

    public SessionDetailModel(IIdentitySessionAppService identitySessionAppService)
    {
        IdentitySessionAppService = identitySessionAppService;
    }

    public virtual async Task OnGetAsync()
    {
        Session = await IdentitySessionAppService.GetAsync(Id);
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
