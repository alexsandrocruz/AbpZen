using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Components.ApplicationLayout.SideMenu.MainHeader
{
    public partial class MainHeaderToolbarUserMenu : IDisposable
    {
        [Inject]
        protected IMenuManager MenuManager { get; set; }

        [Inject]
        protected ICurrentUser CurrentUser { get; set; }

        [Inject]
        protected ICurrentTenant CurrentTenant { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected IStringLocalizer<LeptonXResource> L { get; set; }

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        protected ApplicationMenu UserMenu { get; set; }

        protected Guid? UserId { get; set; }

        protected string UserName { get; set; }

        protected string TenantName { get; set; }

        protected string ProfileImageUrl { get; set; }

        protected string UserFullName { get; set; }

        protected string UserEmail { get; set; }

        protected async override Task OnInitializedAsync()
        {
            UserMenu = await MenuManager.GetAsync(StandardMenus.User);

            UserId = CurrentUser.Id;
            UserName = CurrentUser.UserName;
            UserFullName = CalculateUserFullName();
            TenantName = CurrentTenant.Name;
            UserEmail = CurrentUser.Email;

            if (UserId != null)
            {
                ProfileImageUrl = $"api/account/profile-picture-file/{UserId}";
            }

            Navigation.LocationChanged += OnLocationChanged;
        }

        protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        protected virtual string CalculateUserFullName()
        {
            //TODO: Should we move this logic to some extension method for the ICurrentUser?

            var fullName = new StringBuilder();

            if (!CurrentUser.Name.IsNullOrEmpty())
            {
                fullName.Append(CurrentUser.Name);
            }

            if (!CurrentUser.SurName.IsNullOrEmpty())
            {
                if (fullName.Length > 0)
                {
                    fullName.Append(' ');
                }

                fullName.Append(CurrentUser.SurName);
            }

            if (fullName.Length == 0)
            {
                fullName.Append(CurrentUser.UserName);
            }

            return fullName.ToString();
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= OnLocationChanged;
        }
    }
}
