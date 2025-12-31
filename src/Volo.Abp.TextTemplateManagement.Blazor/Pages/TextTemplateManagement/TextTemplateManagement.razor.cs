using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TextTemplateManagement.Authorization;
using Volo.Abp.TextTemplateManagement.TextTemplates;
using Volo.Abp.BlazoriseUI.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using System;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.TextTemplateManagement.Blazor.Pages.TextTemplateManagement.Components;

namespace Volo.Abp.TextTemplateManagement.Blazor.Pages.TextTemplateManagement;

public class TextTemplateFilterModel
{
    private string _filter = null;

    public string Filter {
        get => string.IsNullOrWhiteSpace(_filter) ? string.Empty : _filter;
        set => _filter = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}

public partial class TextTemplateManagement
{
    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int? TotalCount;
    protected string UpdatePolicyName;
    protected bool HasUpdatePermission;
    protected DataGridEntityActionsColumn<TemplateDefinitionDto> EntityActionsColumn;
    protected List<BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new();

    protected TextTemplateFilterModel GetListInput = new();

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected IReadOnlyList<TemplateDefinitionDto> TemplateDefinitions;

    protected PageToolbar Toolbar { get; } = new();
    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();
    protected List<TableColumn> TextTemplateManagementTableColumns => TableColumns.Get<TextTemplateManagement>();

    [Inject]
    protected ITemplateDefinitionAppService TemplateDefinitionAppService { get; set; }

    [Inject]
    protected ITemplateContentAppService TemplateContentAppService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    public TextTemplateManagement()
    {
        UpdatePolicyName = TextTemplateManagementPermissions.TextTemplates.EditContents;
    }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await GetEntitiesAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual async Task SetPermissionsAsync()
    {
        HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
    }

    protected virtual async Task GetEntitiesAsync()
    {
        var input = await CreateGetTemplateDefinitionListInputAsync();
        var result = await TemplateDefinitionAppService.GetListAsync(input);
        TemplateDefinitions = result.Items;
        TotalCount = (int?)result.TotalCount;
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<TemplateDefinitionDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
    }

    protected virtual Task<GetTemplateDefinitionListInput> CreateGetTemplateDefinitionListInputAsync()
    {
        var input = new GetTemplateDefinitionListInput
        {
            Sorting = CurrentSorting,
            SkipCount = (CurrentPage - 1) * PageSize,
            MaxResultCount = PageSize,
            FilterText = GetListInput.Filter
        };

        return Task.FromResult(input);
    }

    protected virtual async Task FilterChangedAsync(string filter)
    {
        GetListInput.Filter = filter;
        await SearchEntitiesAsync();
    }

    protected virtual void NavigateToEdit(TemplateDefinitionDto templateDefinition)
    {
        var baseRoute = templateDefinition.IsInlineLocalized
            ? "/text-templates/contents/inline?name="
            : "/text-templates/contents?name=";

        NavigationManager.NavigateTo($"{baseRoute}{templateDefinition.Name}");
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:TextTemplates"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TextTemplateManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["EditContents"],
                        Visible = (data) => HasUpdatePermission,
                        Color = Color.Primary,
                        Clicked = (data) =>
                        {
                            NavigateToEdit(data.As<TemplateDefinitionDto>());
                            return Task.CompletedTask;
                        }
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        TextTemplateManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<TextTemplateManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Data = nameof(TemplateDefinitionDto.DisplayName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["IsInlineLocalized"],
                        Data = nameof(TemplateDefinitionDto.IsInlineLocalized),
                        Sortable = true,
                        Component = typeof(IsInlineColumnComponent)
                    },
                    new TableColumn
                    {
                        Title = L["Layout"],
                        Data = nameof(TemplateDefinitionDto.Layout),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DefaultCultureName"],
                        Data = nameof(TemplateDefinitionDto.DefaultCultureName),
                        Sortable = true,
                    },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        return ValueTask.CompletedTask;
    }
}
