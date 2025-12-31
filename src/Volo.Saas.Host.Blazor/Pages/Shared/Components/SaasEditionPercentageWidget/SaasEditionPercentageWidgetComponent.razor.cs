using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using Microsoft.AspNetCore.Components;

namespace Volo.Saas.Host.Blazor.Pages.Shared.Components.SaasEditionPercentageWidget;

public partial class SaasEditionPercentageWidgetComponent
{
    [Inject]
    protected IEditionAppService EditionAppService { get; set; }
    
    protected PieChart<int> BarChart;
    
    protected readonly PieChartOptions Options = new() 
    {
        Responsive = true,
        MaintainAspectRatio = false
    };

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RefreshAsync();
        }
    }

    public virtual async Task RefreshAsync()
    {
        await BarChart.Clear();
        var statistic = await EditionAppService.GetUsageStatisticsAsync();
        
        var dataset = new PieChartDataset<int>
        {
            Data = statistic.Data.Select(x =>　x.Value).ToList(),
            BackgroundColor = GetRandomBackgroundColor(statistic.Data.Count)
        };

        
        await BarChart.AddLabelsDatasetsAndUpdate(statistic.Data.Select(x => x.Key).ToList(), dataset);
    }
    
    protected virtual List<string> GetRandomBackgroundColor(int count)
    {
        var backgroundColor = new List<string>();
        for (var i = 0; i < count; i++)
        {
            var r = Convert.ToByte(((i + 5) * (i + 5) * 474) % 255);
            var g = Convert.ToByte(((i + 5) * (i + 5) * 1600) % 255);
            var b = Convert.ToByte(((i + 5) * (i + 5) * 84065) % 255);
            
            backgroundColor.Add(ChartColor.FromRgba(r, g, b, 0.7f));
        }

        return backgroundColor;
    }
}