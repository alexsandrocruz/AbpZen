using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sapienza.Dominus.Web.Components.Toolbar.Impersonation;
using Sapienza.Dominus.Web.Components.Toolbar.LoginLink;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;

namespace Sapienza.Dominus.Web.Menus
{
    public class DominusToolbarContributor : IToolbarContributor
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
