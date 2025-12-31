using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users;

public class SessionsModal : IdentityPageModel
{

    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }
    
    protected IIdentityUserAppService IdentityUserAppService { get; } 
    
    public IdentityUserDto UserInfo { get; set; }
    
    public SessionsModal(IIdentityUserAppService identityUserAppService)
    {
        IdentityUserAppService = identityUserAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        UserInfo = await IdentityUserAppService.GetAsync(UserId);
        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
