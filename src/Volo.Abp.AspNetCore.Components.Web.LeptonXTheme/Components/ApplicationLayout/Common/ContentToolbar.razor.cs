using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Layout;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.Common;

public partial class ContentToolbar
{
    [Inject] protected PageLayout PageLayout { get; set; }

    protected List<RenderFragment> ToolbarItemRenders { get; } = new();

    protected override Task OnInitializedAsync()
    {
        PageLayout.ToolbarItems.CollectionChanged += async (s, e) => await RenderAsync(); // Handles ToolbarItems Changes
        PageLayout.PropertyChanged += async (s, e) => await InvokeAsync(StateHasChanged); // Handles Title Changes
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RenderAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected virtual async Task RenderAsync()
    {
        ToolbarItemRenders.Clear();
        foreach (var item in PageLayout.ToolbarItems.ToImmutableList())
        {
            var sequence = 0;
            ToolbarItemRenders.Add(builder =>
            {
                builder.OpenComponent(sequence, item.ComponentType);
                if (item.Arguments != null)
                {
                    foreach (var argument in item.Arguments)
                    {
                        sequence++;
                        builder.AddAttribute(sequence, argument.Key, argument.Value);
                    }
                }
                builder.CloseComponent();
            });
        }

        await InvokeAsync(StateHasChanged);
    }
}