using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.LeptonX.Shared;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.TopMenu.MainHeader;

public partial class MainHeaderToolbar
{
    [Inject]
    private IToolbarManager ToolbarManager { get; set; }

    private RenderFragment ToolbarRender { get; set; }

    private List<RenderFragment> ToolbarItemRenders { get; set; } = new List<RenderFragment>();

    protected override async Task OnInitializedAsync()
    {
        var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

		var leptonxToolbar = await ToolbarManager.GetAsync(LeptonXToolbars.Main);

		toolbar.Items.AddRange(leptonxToolbar.Items);

		ToolbarItemRenders.Clear();

        foreach (var item in toolbar.Items.OrderBy(x => x.Order))
        {
            ToolbarItemRenders.Add(builder =>
            {
                builder.OpenComponent(0, item.ComponentType);
                builder.CloseComponent();
            });
        }
    }
}