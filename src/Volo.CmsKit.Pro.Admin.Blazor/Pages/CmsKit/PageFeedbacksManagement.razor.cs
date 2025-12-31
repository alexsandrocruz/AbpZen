using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class PageFeedbacksManagement
{
    [Inject] protected IPageFeedbackAdminAppService PageFeedbackAdminAppService { get; set; }

    protected List<BreadcrumbItem> BreadcrumbItems = new(2);

    protected PageToolbar Toolbar { get; } = new();

    protected GetPageFeedbackListInput GetListInput = new();

    protected IReadOnlyList<PageFeedbackDto> PageFeedbackDtos = Array.Empty<PageFeedbackDto>();

    protected TableColumnDictionary TableColumns = new();

    protected EntityActionDictionary EntityActions = new();

    protected List<TableColumn> PageFeedbacksManagementTableColumns => TableColumns.Get<PageFeedbacksManagement>();

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;

    protected int? TotalCount;

    protected Modal EditModal;

    protected Modal ViewModal;

    protected Modal SettingsModal;

    protected PageFeedbackDto SelectedPageFeedbackDto = new();
    protected List<UpdatePageFeedbackSettingDto> SettingDtos { get; set; } = new();
    protected UpdatePageFeedbackSettingDto DefaultSetting { get; set; } = new();

    public bool HasUpdatePermission { get; set; }
    public bool HasDeletePermission { get; set; }
    public bool HasSettingsPermission { get; set; }

    protected SearchViewModel SearchInput { get; set; } = new();

    protected List<string> EntityTypes { get; set; } = new();

    protected async override Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await SetPermissionsAsync();
        EntityTypes = await PageFeedbackAdminAppService.GetEntityTypesAsync();
        PageFeedbackDtos = (await PageFeedbackAdminAppService.GetListAsync(new GetPageFeedbackListInput())).Items;
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
        HasUpdatePermission = await AuthorizationService.IsGrantedAsync(CmsKitProAdminPermissions.PageFeedbacks.Update);
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(CmsKitProAdminPermissions.PageFeedbacks.Delete);
        HasSettingsPermission =
            await AuthorizationService.IsGrantedAsync(CmsKitProAdminPermissions.PageFeedbacks.Settings);
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["PageFeedbacks"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["MailSettings"],
            OpenSettingsModalAsync,
            IconName.Mail,
            requiredPolicyName: CmsKitProAdminPermissions.PageFeedbacks.Settings);

        return ValueTask.CompletedTask;
    }

    protected virtual async Task SearchPageFeedbacksAsync()
    {
        CurrentPage = 1;

        await GetPageFeedbacksAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task GetPageFeedbacksAsync()
    {
        try
        {
            GetListInput.EntityType = SearchInput.EntityType == SearchViewModel.NullSelectionValue ? null : SearchInput.EntityType;
            GetListInput.IsUseful = SearchInput.IsUseful == SearchViewModel.NullSelectionValue ? null : SearchInput.IsUseful == "True";
            GetListInput.IsHandled = SearchInput.IsHandled == SearchViewModel.NullSelectionValue ? null : SearchInput.IsHandled == "True";
            GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
            GetListInput.MaxResultCount = PageSize;

            var result = await PageFeedbackAdminAppService.GetListAsync(GetListInput);
            PageFeedbackDtos = result.Items;
            TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PageFeedbackDto> e)
    {
        CurrentPage = e.Page;

        await GetPageFeedbacksAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<PageFeedbacksManagement>()
            .AddRange(new EntityAction[] {
                new() {
                    Text = L["View"],
                    Clicked = async (data) => { await OpenViewModalAsync(data.As<PageFeedbackDto>().Id); }
                },
                new() {
                    Text = L["Edit"],
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<PageFeedbackDto>().Id); },
                    Visible = (data) => HasUpdatePermission,
                },
                new() {
                    Text = L["Delete"],
                    Clicked = async (data) => { await DeletePageFeedbackAsync(data.As<PageFeedbackDto>().Id); },
                    ConfirmationMessage = (data) => L["PageFeedbackDeletionConfirmationMessage"],
                    Visible = (data) => HasDeletePermission
                }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        PageFeedbacksManagementTableColumns
            .AddRange(new TableColumn[] {
                new() { Title = L["Actions"], Actions = EntityActions.Get<PageFeedbacksManagement>() },
                new() { Title = L["EntityType"], Data = nameof(PageFeedbackDto.EntityType) },
                new() { Title = L["EntityId"], Data = nameof(PageFeedbackDto.EntityId) },
                new() { Title = L["URL"], Data = nameof(PageFeedbackDto.Url), ValueConverter = (data => data is string url ? $"<a href=\"{url}\" target=\"_blank\">{url[..128]}</a>" : "") }, 
                new() {
                    Title = L["IsUseful"],
                    Data = nameof(PageFeedbackDto.IsUseful),
                    ValueConverter = (data) => data is true
                        ? "<i class=\"fa fa-thumbs-up text-primary\"></i>"
                        : "<i class=\"fa fa-thumbs-down text-muted opacity-25\"></i>"
                },
                new() {
                    Title = L["IsHandled"],
                    Data = nameof(PageFeedbackDto.IsHandled),
                    ValueConverter = (data) => data is true
                        ? "<i class=\"fa fa-check text-success\"></i>"
                        : ""
                },
                new() { Title = L["CreationTime"], Data = nameof(PageFeedbackDto.CreationTime) }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual async Task OpenEditModalAsync(Guid id)
    {
        try
        {
            SelectedPageFeedbackDto = await PageFeedbackAdminAppService.GetAsync(id);
            await InvokeAsync(EditModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual Task ClosingEditModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseEditModalAsync()
    {
        return InvokeAsync(EditModal.Hide);
    }

    protected virtual async Task OpenViewModalAsync(Guid id)
    {
        try
        {
            SelectedPageFeedbackDto = await PageFeedbackAdminAppService.GetAsync(id);
            await InvokeAsync(ViewModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual Task ClosingViewModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseViewModalAsync()
    {
        return InvokeAsync(ViewModal.Hide);
    }

    protected virtual async Task OpenSettingsModalAsync()
    {
        try
        {
            SettingDtos.Clear();
            var settings =
                ObjectMapper.Map<List<PageFeedbackSettingDto>, List<UpdatePageFeedbackSettingDto>>(
                    await PageFeedbackAdminAppService.GetSettingsAsync());

            DefaultSetting = settings.FirstOrDefault(x => x.EntityType == null) ??
                                   new UpdatePageFeedbackSettingDto();

            foreach (var entityType in EntityTypes)
            {
                SettingDtos.Add(settings.FirstOrDefault(x => x.EntityType == entityType) ??
                                new UpdatePageFeedbackSettingDto { EntityType = entityType });
            }

            await InvokeAsync(SettingsModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual Task ClosingSettingsModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseSettingsModalAsync()
    {
        return InvokeAsync(SettingsModal.Hide);
    }
    
    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            var settings = SettingDtos.Append(DefaultSetting).ToList();

            await PageFeedbackAdminAppService.UpdateSettingsAsync(
                new UpdatePageFeedbackSettingsInput(settings)
            );

            await CloseSettingsModalAsync();
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual async Task UpdatePageFeedbackAsync()
    {
        try
        {
            var updatePageFeedbackDto =
                ObjectMapper.Map<PageFeedbackDto, UpdatePageFeedbackDto>(SelectedPageFeedbackDto);
            await PageFeedbackAdminAppService.UpdateAsync(SelectedPageFeedbackDto.Id, updatePageFeedbackDto);
            await CloseEditModalAsync();
            await GetPageFeedbacksAsync();
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual async Task DeletePageFeedbackAsync(Guid id)
    {
        try
        {
            await PageFeedbackAdminAppService.DeleteAsync(id);
            await GetPageFeedbacksAsync();
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    public class SearchViewModel
    {
        public string EntityType { get; set; } = NullSelectionValue;
        public string IsUseful { get; set; } = NullSelectionValue;
        public string IsHandled { get; set;} = NullSelectionValue;

        public const string NullSelectionValue = "empty_value";
    }
}