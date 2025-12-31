using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class EmailConfirmationModel : AccountPageModel
{
    private readonly IAccountAppService _accountAppService;

    public EmailConfirmationModel(IAccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    [Required]
    [BindProperty(SupportsGet = true)]
    public Guid UserId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string ConfirmationToken { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public bool EmailConfirmed { get; set; }

    public bool InvalidToken { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ReturnUrl = await GetRedirectUrlAsync(ReturnUrl, ReturnUrlHash);

        try
        {
            var user = await UserManager.GetByIdAsync(UserId);
            if (user.EmailConfirmed)
            {
                EmailConfirmed = true;
                return Page();
            }

            ValidateModel();
            InvalidToken = !await AccountAppService.VerifyEmailConfirmationTokenAsync(
                new VerifyEmailConfirmationTokenInput()
                {
                    UserId = UserId,
                    Token = ConfirmationToken
                }
            );

            if (!InvalidToken)
            {
                await _accountAppService.ConfirmEmailAsync(new ConfirmEmailInput
                {
                    UserId = UserId,
                    Token = ConfirmationToken
                });

                EmailConfirmed = true;
            }
        }
        catch (Exception e)
        {
            if (e is AbpIdentityResultException && !string.IsNullOrWhiteSpace(e.Message))
            {
                Alerts.Warning(GetLocalizeExceptionMessage(e));
                return Page();
            }

            if (e is AbpValidationException)
            {
                return Page();
            }

            throw;
        }

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
