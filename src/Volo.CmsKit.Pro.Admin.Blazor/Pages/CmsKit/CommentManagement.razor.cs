using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class CommentManagement
{
    [Inject]
    protected ICommentAdminAppService CommentAdminAppService { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    protected TableColumnDictionary TableColumns = new TableColumnDictionary();

    protected EntityActionDictionary EntityActions = new EntityActionDictionary();
    
    protected List<TableColumn> CommentsManagementTableColumns => TableColumns.Get<CommentManagement>();

    protected CommentGetListInput GetListInput = new CommentGetListInput();

    protected List<CommentWithAuthorViewModel> Comments = new List<CommentWithAuthorViewModel>();
    
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    public IReadOnlyList<DateTime?> DataRange { get; set; } = new List<DateTime?>(2) { DateTime.Now.AddDays(-7).Date };

    protected int CurrentPage = 1;
    
    protected string CurrentSorting;
    
    protected int? TotalCount;

    protected bool HasDeletePermission;

    protected async override Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Comments.Delete);
        await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual async Task GetCommentsAsync()
    {
        try
        {
            GetListInput.Sorting = CurrentSorting;
            GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
            GetListInput.MaxResultCount = PageSize;
            GetListInput.CreationStartDate = DataRange.FirstOrDefault();
            GetListInput.CreationEndDate = DataRange.Count > 1 ? DataRange.LastOrDefault() : null;
            
            var result = await CommentAdminAppService.GetListAsync(GetListInput);
            Comments = ObjectMapper.Map<List<CommentWithAuthorDto>,List<CommentWithAuthorViewModel>>(result.Items.ToList());
            TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task SearchCommentsAsync()
    {
        CurrentPage = 1;

        await GetCommentsAsync();

        await InvokeAsync(StateHasChanged);
    }
    
    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CommentWithAuthorViewModel> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetCommentsAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Comments"]));
        return ValueTask.CompletedTask;
    }

    protected virtual Task OpenCommentDetailsAsync(CommentWithAuthorViewModel comment)
    {
        NavigationManager.NavigateTo("Cms/Comments/details/" + comment.Id);
        return Task.CompletedTask;
    }
    
    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<CommentManagement>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Details"],
                    Clicked = async (data) => await OpenCommentDetailsAsync(data.As<CommentWithAuthorViewModel>()),
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteCommentAsync(data.As<CommentWithAuthorViewModel>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage()
                }
            });
        
        return ValueTask.CompletedTask;
    }

    protected virtual async Task DeleteCommentAsync(CommentWithAuthorViewModel comment)
    {
        await CommentAdminAppService.DeleteAsync(comment.Id);
        await Message.Success(L["DeletedSuccessfully"]);
        await GetCommentsAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual string GetDeleteConfirmationMessage()
    {
        return L["CommentDeletionConfirmationMessage"];
    }
    
    protected virtual ValueTask SetTableColumnsAsync()
    {
        CommentsManagementTableColumns
            .AddRange(new TableColumn[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = EntityActions.Get<CommentManagement>()
                },
                new TableColumn
                {
                    Title = L["Username"],
                    Data = nameof(CommentWithAuthorViewModel.UserName)
                },
                new TableColumn
                {
                    Title = L["EntityType"],
                    Data = nameof(CommentWithAuthorViewModel.EntityType)
                },
                new TableColumn
                {
                    Title = L["Text"],
                    Data = nameof(CommentWithAuthorViewModel.Text)
                },
                new TableColumn
                {
                    Title = L["CreationTime"],
                    Data = nameof(CommentWithAuthorViewModel.CreationTime),
                    Sortable = true
                }
            });
        
        return ValueTask.CompletedTask;
    }
    
    public class CommentWithAuthorViewModel
    {
        public Guid Id { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string Text { get; set; }

        public Guid? RepliedCommentId { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserName { get; set; }
    }
}