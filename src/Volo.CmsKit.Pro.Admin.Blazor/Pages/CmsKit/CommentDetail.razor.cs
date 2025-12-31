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

public partial class CommentDetail
{
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
    
    [Parameter]
    public Guid Id { get; set; }
    
    [Inject]
    protected ICommentAdminAppService CommentAdminAppService { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected CommentWithAuthorDto Comment = new CommentWithAuthorDto();

    protected CommentGetListInput GetListInput = new CommentGetListInput();
    
    protected List<CommentManagement.CommentWithAuthorViewModel> Comments = new List<CommentManagement.CommentWithAuthorViewModel>();
    
    protected TableColumnDictionary TableColumns = new TableColumnDictionary();

    protected EntityActionDictionary EntityActions = new EntityActionDictionary();
    
    protected List<TableColumn> CommentsManagementTableColumns => TableColumns.Get<CommentDetail>();
    
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;
    
    protected string CurrentSorting;
    
    protected int? TotalCount;

    protected bool HasDeletePermission;

    protected async override Task OnInitializedAsync()
    {
        GetListInput.RepliedCommentId = Id;
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await GetCommentAsync();
        await InvokeAsync(StateHasChanged);
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Comments.Delete);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual async Task GetCommentAsync()
    {
        Comment = await CommentAdminAppService.GetAsync(Id);
    }
    
    protected virtual async Task GetCommentsAsync()
    {
        try
        {
            GetListInput.Sorting = CurrentSorting;
            GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
            GetListInput.MaxResultCount = PageSize;
            
            var result = await CommentAdminAppService.GetListAsync(GetListInput);
            Comments = ObjectMapper.Map<List<CommentWithAuthorDto>, List<CommentManagement.CommentWithAuthorViewModel>>(result.Items.ToList());
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
    
    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CommentManagement.CommentWithAuthorViewModel> e)
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

    protected virtual async Task OpenCommentDetailsAsync(CommentManagement.CommentWithAuthorViewModel comment)
    {
        Id = comment.Id;
        GetListInput.RepliedCommentId = comment.Id;
        await GetCommentAsync();
        await GetCommentsAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<CommentDetail>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Details"],
                    Clicked = async (data) => await OpenCommentDetailsAsync(data.As<CommentManagement.CommentWithAuthorViewModel>())
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteCommentAsync(data.As<CommentManagement.CommentWithAuthorViewModel>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage()
                }
            });
        
        return ValueTask.CompletedTask;
    }

    protected virtual async Task DeleteCommentAsync(CommentManagement.CommentWithAuthorViewModel comment)
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
                    Actions = EntityActions.Get<CommentDetail>()
                },
                new TableColumn
                {
                    Title = L["Username"],
                    Data = nameof(CommentManagement.CommentWithAuthorViewModel.UserName)
                },
                new TableColumn
                {
                    Title = L["Text"],
                    Data = nameof(CommentManagement.CommentWithAuthorViewModel.Text)
                },
                new TableColumn
                {
                    Title = L["CreationTime"],
                    Data = nameof(CommentManagement.CommentWithAuthorViewModel.CreationTime)
                }
            });
        
        return ValueTask.CompletedTask;
    }
}