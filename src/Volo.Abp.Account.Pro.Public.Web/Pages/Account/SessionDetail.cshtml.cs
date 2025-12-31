using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class SessionDetailModel : AccountPageModel
{
    public IdentitySessionDto Session { get; private set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    protected IAccountSessionAppService AccountSessionAppService { get; }

    public SessionDetailModel(IAccountSessionAppService accountSessionAppService)
    {
        AccountSessionAppService = accountSessionAppService;
    }

    public virtual async Task OnGetAsync()
    {
        Session = await AccountSessionAppService.GetAsync(Id);
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
