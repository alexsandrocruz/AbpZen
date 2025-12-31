using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class LockedOut : AccountPageModel
{
    public static string LockedUserScheme = "Abp.LockedUser";
    
    public UserInfoModel UserInfo { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        UserInfo = await RetrieveConfirmUser();
     
        if (UserInfo == null)
        {
            return await RedirectToLoginPageAsync();
        }

        if (UserInfo.TenantId != CurrentTenant.Id)
        {
            return await RedirectToLoginPageAsync();
        }

        
        if (!UserInfo.LockoutEnabled || !UserInfo.IsLocked)
        {
            return await RedirectToLoginPageAsync();
        }
        
        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
    
    protected virtual async Task<IActionResult> RedirectToLoginPageAsync()
    {
        if (UserInfo != null)
        {
            // Try to cleanup locked user id cookies
            await HttpContext.SignOutAsync(LockedUserScheme);
        }

        return RedirectToPage("./Login", new {
            ReturnUrl = ReturnUrl,
            ReturnUrlHash = ReturnUrlHash
        });
    }
    
    protected virtual async Task<UserInfoModel> RetrieveConfirmUser()
    {
        var result = await HttpContext.AuthenticateAsync(LockedUserScheme);
        if (result?.Principal != null)
        {
            var userId = result.Principal.FindUserId();
            if (userId == null)
            {
                return null;
            }

            var tenantId = result.Principal.FindTenantId();

            using (CurrentTenant.Change(tenantId))
            {
                var user = await UserManager.FindByIdAsync(userId.Value.ToString());
                if (user == null)
                {
                    return null;
                }

                return new UserInfoModel
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    LockoutEnd = user.LockoutEnd,
                    LockoutEnabled = user.LockoutEnabled
                };
            }
        }

        return null;
    }
    
    public class UserInfoModel
    {
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        
        public bool LockoutEnabled { get; set; }
        
        public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTimeOffset.Now;
    }
}
