using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.AuditLogging.Blazor.Pages.Shared.ErrorRateWidget;

public partial class AuditLoggingErrorRateWidgetComponent
{
    [Inject]
    protected IAuditLogsAppService AuditLogsAppService { get; set; }
    
    protected PieChart<long> PieChart;

    protected readonly List<string> BackgroundColors = new() 
    {
        ChartColor.FromRgba(200, 50, 50, 0.7f),
        ChartColor.FromRgba(50, 200, 50, 0.7f)
    };
    
    [Parameter]
    public DateTime StartDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> StartDateChanged { get; set; }
    
    [Parameter]
    public DateTime EndDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> EndDateChanged { get; set; }
    
    protected readonly PieChartOptions Options = new() 
    {
        Responsive = true,
        MaintainAspectRatio = false,
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
        await PieChart.Clear();
        var input = new GetErrorRateFilter {StartDate = StartDate, EndDate = EndDate};
        var statistic = await AuditLogsAppService.GetErrorRateAsync(input);
        
        var dataset = new PieChartDataset<long>
        {
            Data = statistic.Data.Select(x =>　x.Value).ToList(),
            BackgroundColor = BackgroundColors,
        };
        
        await PieChart.AddLabelsDatasetsAndUpdate(statistic.Data.Select(x => x.Key).ToList(), dataset);
    }
}