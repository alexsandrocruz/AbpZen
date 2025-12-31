using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.CmsKit.Admin.Polls;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.WidgetComponents;

public partial class PollsComponent
{
    [Inject]
    protected IPollAdminAppService PollAdminAppService { get; set; }

    protected PollsViewModel Model { get; set; } = new PollsViewModel();

    protected async override Task OnInitializedAsync()
    {
        await GetPullListAsync();
    }

    protected virtual async Task GetPullListAsync()
    {
        var polls = await PollAdminAppService.GetListAsync(new GetPollListInput());

        Model = new PollsViewModel()
        {
            Widgets = polls.Items.Where(x=> !x.Name.IsNullOrWhiteSpace()).ToDictionary(x => x.Name, x => x.Code)
        };
    }

    public class PollsViewModel
    {
        public Dictionary<string, string> Widgets { get; set; } = new();

        public string Widget { get; set; }
    }
}