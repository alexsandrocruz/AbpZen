using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging.Components;
using Volo.Abp.AuditLogging.Localization;

namespace Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging;

public partial class AuditLogs
{
    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int? TotalCount;
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    public string DateFormat => CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;

    public string TimeFormat => CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;

    public bool TimeAs24hr => !CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern.Contains("tt");

    [Inject]
    protected IAuditLogsAppService AppService { get; set; }

    protected Modal DetailModal;

    protected const string DetailModalDefaultTabName = "Overall";

    protected string DetailModalSelectedTab = DetailModalDefaultTabName;

    protected AuditLogDto AuditLogDetail = new AuditLogDto();

    protected Dictionary<Guid, bool> AuditLogDetailActionPanelStatus;
    protected Dictionary<Guid, bool> AuditLogDetailEntityChangesPanelStatus;

    protected GetAuditLogListDto Filter = new GetAuditLogListDto
    {
        StartTime = DateTime.Now.AddDays(-7)
    };

    protected IReadOnlyList<AuditLogDto> AuditLogList;

    // Those nullable variables are created for null SelectItem
    // TODO: Find a better way
    protected readonly bool? NullBoolValue = null;

    protected readonly int NullStatusCodeValue = -1;

    protected int StatusCodeValue = -1;

    protected const string NullStringValue = "empty_value";
    
    protected string HasException { get; set; } = NullStringValue;
    
    protected string HttpMethod { get; set; }

    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();
    protected List<TableColumn> AuditLogsTableColumns => TableColumns.Get<AuditLogs>();

    protected override async Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
    }
    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
    }

    protected virtual async Task GetEntitiesAsync()
    {
        Filter.HttpStatusCode = StatusCodeValue == NullStatusCodeValue ? null : (HttpStatusCode?)StatusCodeValue;
        Filter.HasException = HasException == NullStringValue ? null : HasException == "True";
        Filter.HttpMethod = HttpMethod == NullStringValue ? null : HttpMethod;
        Filter.Sorting = CurrentSorting;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.MaxResultCount = PageSize;

        Filter.UserName = Filter.UserName?.Trim();
        Filter.Url = Filter.Url?.Trim();
        Filter.ApplicationName = Filter.ApplicationName?.Trim();
        Filter.CorrelationId = Filter.CorrelationId?.Trim();
        Filter.ClientIpAddress = Filter.ClientIpAddress?.Trim();

        var result = await AppService.GetListAsync(Filter);
        AuditLogList = result.Items;

        TotalCount = (int?)result.TotalCount;
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AuditLogDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();
    }

    protected virtual List<string> GetHttpMethods()
    {
        return new List<string>
            {
                "GET",
                "POST",
                "DELETE",
                "PUT",
                "HEAD",
                "CONNECT",
                "OPTIONS",
                "TRACE",
            };
    }

    protected virtual List<HttpStatusCode> GetHttpStatusCodeList()
    {
        return Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Distinct().ToList();
    }

    protected virtual Color GetHttpMethodBadgeColor(string method)
    {
        return method switch
        {
            "GET" => Color.Dark,
            "POST" => Color.Success,
            "DELETE" => Color.Danger,
            "PUT" => Color.Warning,
            _ => Color.Default
        };
    }

    protected virtual async Task OpenDetailModalAsync(Guid id)
    {
        DetailModalSelectedTab = DetailModalDefaultTabName;

        AuditLogDetailActionPanelStatus = new Dictionary<Guid, bool>();
        AuditLogDetailEntityChangesPanelStatus = new Dictionary<Guid, bool>();

        AuditLogDetail = await AppService.GetAsync(id);
        await InvokeAsync(async () => await DetailModal.Show());
    }

    protected virtual async Task CloseDetailModalAsync()
    {
        await DetailModal.Hide();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual string SerializeDictionary(Dictionary<string, object> dictionary)
    {
        return JsonSerializer.Serialize(dictionary, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<AuditLogs>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Detail"],
                        Clicked = async (data) => await OpenDetailModalAsync(data.As<AuditLogDto>().Id),
                        Icon = "fa fa-eye",
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        AuditLogsTableColumns
             .AddRange(new TableColumn[]
             {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<AuditLogs>()
                    },
                    new TableColumn
                    {
                        Title = L["HttpRequest"],
                        Data = nameof(AuditLogDto.HttpMethod),
                        Sortable = true,
                        Component = typeof(HttpMethodColumnComponent)
                    },
                    new TableColumn
                    {
                        Title = L["UserName"],
                        Data = nameof(AuditLogDto.UserName),
                        Sortable = true,
                        ValueConverter = (data) =>
                        {
                            var auditLog = (data as AuditLogDto);

                            var impersonator = (!auditLog.ImpersonatorTenantName.IsNullOrWhiteSpace() ? auditLog.ImpersonatorTenantName : "") + (!auditLog.ImpersonatorUserName.IsNullOrWhiteSpace() ? ("\\" + auditLog.ImpersonatorUserName) : "");
                            if (!impersonator.IsNullOrWhiteSpace())
                            {
                                impersonator = " (" + impersonator + ")";
                            }

                            var tenant = !auditLog.TenantName.IsNullOrWhiteSpace() ? (auditLog.TenantName + "\\") : "";
                            return tenant + auditLog.UserName + impersonator;
                        }
                    },
                    new TableColumn
                    {
                        Title = L["IpAddress"],
                        Data = nameof(AuditLogDto.ClientIpAddress),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["Time"],
                        Data = nameof(AuditLogDto.ExecutionTime),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DurationMs"],
                        Data = nameof(AuditLogDto.ExecutionDuration),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["ApplicationName"],
                        Data = nameof(AuditLogDto.ApplicationName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["CorrelationId"],
                        Data = nameof(AuditLogDto.CorrelationId),
                        Sortable = true,
                    },
             });

        return ValueTask.CompletedTask;
    }

    protected virtual string GetHttpStatusCodeText(HttpStatusCode statusCode)
    {
        return $"{(int)statusCode} - {Regex.Replace(statusCode.ToString(), "([A-Z])", " $1")}";
    }

    protected virtual Color GetHttpStatusCodeBadgeColor(int? statusCode)
    {
        if (statusCode is null)
        {
            return Color.Primary;
        }

        if (statusCode >= 200 && statusCode < 300)
        {
            return Color.Success;
        }

        if (statusCode >= 300 && statusCode < 400)
        {
            return Color.Warning;
        }

        if (statusCode >= 400 && statusCode < 600)
        {
            return Color.Danger;
        }

        return Color.Primary;
    }

    protected virtual Task ClosingDetailModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }
    
    protected virtual async Task ClearFilterAsync()
    {
        Filter = new GetAuditLogListDto
        {
            StartTime = DateTime.Now.AddDays(-7)
        };
        StatusCodeValue = NullStatusCodeValue;
        HasException = NullStringValue;
        HttpMethod = NullStringValue;
        CurrentPage = 1;
        await GetEntitiesAsync();
    }
}
