using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.TopMenu.Navigation;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme.Components.ApplicationLayout.TopMenu.Navigation;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(MobileNavbar))]
public class MobileWasmNavbar : MobileNavbar, IDisposable
{
	public const string LogoutMenuName = "UserMenu.TopMenu.Logout";

	protected override string LoginLink => AuthenticationOptions.Value.LoginUrl;

	[Inject]protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; set; }

	[Inject] protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

	[Inject] protected IOptions<AuthenticationOptions> AuthenticationOptions { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
	}

	private async void ApplicationConfigurationChanged()
	{
		await SetMenuAndProfileAsync();
		await InvokeAsync(StateHasChanged);
	}

	protected override async Task SetMenuAndProfileAsync()
	{
		await base.SetMenuAndProfileAsync();

		if (CurrentUser.IsAuthenticated)
		{
			UserMenu.AddItem(new UI.Navigation.ApplicationMenuItem(
				LogoutMenuName,
				L["Logout"],
				AuthenticationOptions.Value.LogoutUrl,
				icon: "bi bi-power"));
		}
	}

	protected override async Task<string> GetProfilePictureURLAsync(Guid userId)
	{
		return (await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AbpAccountPublic"))?.BaseUrl.EnsureEndsWith('/')
			+ $"api/account/profile-picture-file/{userId}";
	}

	public void Dispose()
	{
		ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
	}
}
