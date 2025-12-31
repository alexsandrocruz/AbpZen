using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.Localization;

namespace Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging;

public partial class EntityChanges
{
    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int? TotalCount;
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    [Inject]
    protected IAuditLogsAppService AppService { get; set; }

    protected GetEntityChangesDto Filter = new GetEntityChangesDto
    {
        StartDate = DateTime.Now.AddDays(-7)
    };

    protected int NullEntityChangeValue = -1;
    protected int EntityChangeValue { get; set; } = -1;

    protected IReadOnlyList<EntityChangeDto> EntityChangeList;

    protected Modal DetailModal;

    protected List<EntityChangeWithUsernameDto> EntityHistories;

    protected Dictionary<Guid, bool> EntityChangesPanelStatus;

    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();
    protected List<TableColumn> EntityChangesTableColumns => TableColumns.Get<EntityChanges>();

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
    }

    protected virtual async Task GetEntitiesAsync()
    {
        Filter.EntityChangeType = EntityChangeValue == NullEntityChangeValue ? null : (EntityChangeType?)EntityChangeValue;
        Filter.Sorting = CurrentSorting;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.MaxResultCount = PageSize;

        Filter.EntityTypeFullName = Filter.EntityTypeFullName?.Trim();

        var result = await AppService.GetEntityChangesAsync(Filter);
        EntityChangeList = result.Items;

        TotalCount = (int?)result.TotalCount;
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<EntityChangeDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();
    }

    protected virtual List<EntityChangeType> GetEntityChangeTypeList()
    {
        return Enum.GetValues(typeof(EntityChangeType)).Cast<EntityChangeType>().Distinct().ToList();
    }

    protected virtual async Task OpenDetailModalAsync(Guid id)
    {
        EntityChangesPanelStatus = new Dictionary<Guid, bool>();

        var entityChange = await AppService.GetEntityChangeWithUsernameAsync(id);

        EntityHistories = new List<EntityChangeWithUsernameDto>
            {
                entityChange
            };

        await InvokeAsync(async () => await DetailModal.Show());
    }

    protected virtual async Task OpenDetailModalAsync(string entityTypeFullName, string entityId)
    {
        EntityChangesPanelStatus = new Dictionary<Guid, bool>();

        EntityHistories = await AppService.GetEntityChangesWithUsernameAsync(new EntityChangeFilter
        {
            EntityTypeFullName = entityTypeFullName,
            EntityId = entityId
        });

        await InvokeAsync(async () => await DetailModal.Show());
    }

    protected virtual async Task CloseDetailModalAsync()
    {
        await DetailModal.Hide();
    }

    protected virtual Task ClosingDetailModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<EntityChanges>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["ChangeDetails"],
                        Clicked = async (data) => await OpenDetailModalAsync(data.As<EntityChangeDto>().Id)
                    },
                    new EntityAction
                    {
                        Text = L["FullChangeHistory"],
                        Clicked = async (data) =>
                        {
                            var change = data.As<EntityChangeDto>();
                            await OpenDetailModalAsync(change.EntityTypeFullName,change.EntityId);
                        }
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        EntityChangesTableColumns
             .AddRange(new TableColumn[]
             {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<EntityChanges>()
                    },
                    new TableColumn
                    {
                        Title = L["ChangeTime"],
                        Data = nameof(EntityChangeDto.ChangeTime),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["ChangeType"],
                        Data = nameof(EntityChangeDto.ChangeType),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["TenantId"],
                        Data = nameof(EntityChangeDto.TenantId),
                        Sortable = true,
                        ValueConverter = (data) =>
                        {
                            var change = data.As<EntityChangeDto>();
                            if(!change.TenantId.HasValue)
                            {
                                return "null";
                            }

                            return change.TenantId.ToString();
                        }
                    },
                    new TableColumn
                    {
                        Title = L["EntityId"],
                        Data = nameof(EntityChangeDto.EntityId)
                    },
                    new TableColumn
                    {
                        Title = L["EntityTypeFullName"],
                        Data = nameof(EntityChangeDto.EntityTypeFullName),
                    },
             });

        return ValueTask.CompletedTask;
    }

    protected override async Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
    }
	
	protected virtual async Task OnTimeChangedAsync(IReadOnlyList<DateTime?> dates)
	{
		if (dates?.Count == 2)
        {
            Filter.StartDate = dates.Min();
		    Filter.EndDate = dates.Max();
        }else if (dates?.Count == 1)
        {
            Filter.StartDate = dates[0];
            Filter.EndDate = null;
        }
        else
        {
            Filter.StartDate = null;
            Filter.EndDate = null;
        }

        if (Filter.EndDate.HasValue)
        {
            Filter.EndDate = Filter.EndDate.Value.ClearTime().Add(new TimeSpan(23, 59, 59));
        }

        await SearchEntitiesAsync();
	}
}
