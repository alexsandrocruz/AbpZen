using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.TopMenu.Navigation
{
	public partial class MobileNavbar
	{
		[Inject] protected IStringLocalizer<LeptonXResource> L { get; set; }

		[Inject] protected IMenuManager MenuManager { get; set; }

		[Inject] protected ICurrentUser CurrentUser { get; set; }

		[Inject] protected ICurrentTenant CurrentTenant { get; set; }

		[Inject] protected MainMenuProvider MainMenuProvider { get; set; }

		[Inject] protected IOptions<LeptonXThemeBlazorOptions> Options { get; set; }

		[Inject] protected IToolbarManager ToolbarManager { get; set; }

		protected ApplicationMenu UserMenu { get; set; }

		protected string ProfileImageUrl { get; set; }

		protected List<MenuItemViewModel> SelectedMenuItems { get; set; } = new();

		protected virtual string LoginLink => "account/login";

		protected Toolbar Toolbar { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await SetMenuAndProfileAsync();
		}

		protected virtual async Task SetMenuAndProfileAsync()
		{
			UserMenu = await MenuManager.GetAsync(StandardMenus.User);

			Toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

			var mobileToolbar = await ToolbarManager.GetAsync(LeptonXToolbars.MainMobile);

			Toolbar.Items.AddRange(mobileToolbar.Items);

			var menu = await MainMenuProvider.GetMenuAsync();

			SelectedMenuItems = Options.Value.MobileMenuSelector(menu.Items.AsReadOnly()).Take(2).ToList();

			if (CurrentUser.IsAuthenticated && CurrentUser.Id != null)
			{
				ProfileImageUrl = await GetProfilePictureURLAsync(CurrentUser.GetId());
			}
		}

		protected virtual Task<string> GetProfilePictureURLAsync(Guid userId)
		{
			return Task.FromResult($"api/account/profile-picture-file/{userId}");
		}
	}
}
