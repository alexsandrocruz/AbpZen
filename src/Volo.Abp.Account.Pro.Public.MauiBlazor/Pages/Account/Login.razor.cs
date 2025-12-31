using System.ComponentModel.DataAnnotations;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.Pages.Account;

public partial class Login
{
    [Inject]
    public IExternalAuthService ExternalAuthService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IOptions<OAuthConfigOptions> OAuthConfigOptions { get; set; }

    private LoginInputModel LoginInput { get; set; } = new();

    private Validations LoginValidationsRef;


    protected override async Task OnInitializedAsync()
    {
        if(OAuthConfigOptions.Value.GrantType == "code")
        {
            var result = await ExternalAuthService.LoginAsync(new LoginInput());
            if (result.IsError)
            {
                await Message.Error($"{result.Error} {result.ErrorDescription}");
                return;
            }

            NavigationManager.NavigateTo("/", true);
        }
    }

    private async Task LoginAsync()
    {
        if (LoginValidationsRef == null || await LoginValidationsRef.ValidateAll())
        {
            var result = await ExternalAuthService.LoginAsync(new LoginInput
            {
                UserNameOrEmailAddress = LoginInput.UserNameOrEmailAddress,
                Password = LoginInput.Password
            });

            if (result.IsError)
            {
                await Message.Error($"{result.Error} {result.ErrorDescription}");
                return;
            }
        }

        NavigationManager.NavigateTo("/", true);
    }

    private void Cancel()
    {
        NavigationManager.NavigateTo("/");
    }

    public class LoginInputModel
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
