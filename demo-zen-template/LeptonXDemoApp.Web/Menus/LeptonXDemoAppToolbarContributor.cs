using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LeptonXDemoApp.Web.Components.Toolbar.Impersonation;
using LeptonXDemoApp.Web.Components.Toolbar.LoginLink;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;

namespace LeptonXDemoApp.Web.Menus
{
    public class LeptonXDemoAppToolbarContributor : IToolbarContributor
    {
        public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return Task.CompletedTask;
            }

            if (!context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(LoginLinkViewComponent), order: -1));
            }

            if (context.ServiceProvider.GetRequiredService<ICurrentUser>().FindImpersonatorUserId() != null)
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(ImpersonationViewComponent)));
            }

            return Task.CompletedTask;
        }
    }
}
