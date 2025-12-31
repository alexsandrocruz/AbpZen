using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.Common;

public partial class Breadcrumbs
{
    [Inject] protected PageLayout PageLayout { get; set; }

    [Inject] protected NavigationManager NavigationManager { get; set; }
    protected override Task OnInitializedAsync()
    {
        PageLayout.BreadcrumbItems.CollectionChanged += async (s, e) => await InvokeAsync(StateHasChanged);
        PageLayout.PropertyChanged += async (s, e) => await InvokeAsync(StateHasChanged);

        return base.OnInitializedAsync();
    }
}