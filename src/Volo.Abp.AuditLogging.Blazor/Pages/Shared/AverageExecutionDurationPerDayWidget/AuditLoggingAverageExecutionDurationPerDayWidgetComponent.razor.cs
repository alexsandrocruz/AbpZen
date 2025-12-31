using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.AuditLogging.Blazor.Pages.Shared.AverageExecutionDurationPerDayWidget;

public partial class AuditLoggingAverageExecutionDurationPerDayWidgetComponent
{
    [Inject]
    protected IAuditLogsAppService AuditLogsAppService { get; set; }

    protected BarChart<double> BarChart;
    
    protected readonly List<string> BackgroundColors = new() 
    {
        ChartColor.FromRgba(200, 50, 50, 0.7f)
    };

    protected readonly BarChartOptions Options = new() 
    {
        Scales = new ChartScales 
        {
            Y = new ChartAxis {
                BeginAtZero = true
            }
        },
        Responsive = true,
        MaintainAspectRatio = false
    };
    
    [Parameter]
    public DateTime StartDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> StartDateChanged { get; set; }
    
    [Parameter]
    public DateTime EndDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime> EndDateChanged { get; set; }
    
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
        var statistic = await AuditLogsAppService.GetAverageExecutionDurationPerDayAsync(new GetAverageExecutionDurationPerDayInput
        {
            StartDate = StartDate,
            EndDate = EndDate
        });
        
        var dataset = new BarChartDataset<double>
        {
            Label = L["AverageExecutionDurationInMilliseconds"],
            Data = statistic.Data.Select(x =>　x.Value).ToList(),
            BackgroundColor = BackgroundColors
        };
        
        await BarChart.AddLabelsDatasetsAndUpdate(statistic.Data.Select(x => x.Key).ToList(), dataset);
    }
}