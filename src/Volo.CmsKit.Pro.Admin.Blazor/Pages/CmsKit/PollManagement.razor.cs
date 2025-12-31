using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Guids;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class PollManagement
{
    [Inject]
    protected IGuidGenerator GuidGenerator { get; set; }

    protected List<TableColumn> PollsManagementTableColumns => TableColumns.Get<PollManagement>();
    protected PageToolbar Toolbar { get; } = new();

    protected string SelectedTab = "question";

    protected IReadOnlyList<PollWidgetDto> Widgets = Array.Empty<PollWidgetDto>();

    protected string NewOption;

    protected List<PollOptionDto> PollOptions = new();

    protected GetResultDto PollResultDetails = new();

    protected Modal ResultModal;

    public PollManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitProAdminPermissions.Polls.Create;
        UpdatePolicyName = CmsKitProAdminPermissions.Polls.Update;
        DeletePolicyName = CmsKitProAdminPermissions.Polls.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Polls"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(PollDto entity)
    {
        return string.Format(L["PollDeletionConfirmationMessage"]);
    }

    protected virtual async Task GetWidgetsAsync()
    {
        Widgets = (await AppService.GetWidgetsAsync()).Items;
    }
    
    protected virtual Task OnQuestionTextChangedAsync(string value, bool isCreateModal)
    {
        var name = value.ToLower().Replace(" ", "-");
        if (isCreateModal)
        {
            NewEntity.Question = value;
            NewEntity.Name = name;
        }
        else
        {
            EditingEntity.Question = value;
            EditingEntity.Name = name;
        }
        
        return Task.CompletedTask;
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewPoll"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<PollManagement>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<PollDto>()); }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<PollDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<PollDto>())
                },
                new EntityAction
                {
                    Text = L["ShowResults"],
                    Clicked = async (data) => await OpenResultModalAsync(data.As<PollDto>().Id)
                },
                new EntityAction
                {
                    Text = L["CopyWidgetCode"],
                    Clicked = async (data) =>
                    {
                        var value = $"[Widget Type=\"Poll\" Code=\"{data.As<PollDto>().Code}\"]";
                        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", value);
                        await Notify.Success(L["CopiedWidgetCode"]);
                    }
                },
            });

        return base.SetEntityActionsAsync();
    }

    protected virtual async Task OpenResultModalAsync(Guid id)
    {
        try
        {
            PollResultDetails = await AppService.GetResultAsync(id);
            await InvokeAsync(ResultModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual Task CloseResultModalAsync()
    {
        InvokeAsync(ResultModal.Hide);
        return Task.CompletedTask;
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        PollsManagementTableColumns
            .AddRange(new TableColumn[]
            {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = EntityActions.Get<PollManagement>()
                },
                new TableColumn
                {
                    Title = L["Question"],
                    Data = nameof(PollDto.Question),
                    Sortable = true
                },
                new TableColumn
                {
                    Title = L["Name"],
                    Data = nameof(PollDto.Name),
                    Sortable = true
                },
                new TableColumn
                {
                    Title = L["Code"],
                    Data = nameof(PollDto.Code),
                    Sortable = true
                },
                new TableColumn
                {
                    Title = L["VoteCount"],
                    Data = nameof(PollDto.VoteCount),
                    Sortable = true
                },
            });

        return base.SetTableColumnsAsync();
    }

    protected async override Task OpenCreateModalAsync()
    {
        await GetWidgetsAsync();
        await OnSelectedTabChangedAsync("question");
        await base.OpenCreateModalAsync();
        PollOptions.Clear();
        NewEntity.StartDate = DateTime.Now;
        NewEntity.Code = Path.GetRandomFileName().Replace(".", string.Empty).Substring(0, 8);
    }

    protected override Task CreateEntityAsync()
    {
        if (PollOptions.Count < 2)
        {
            return Message.Warn(L["Poll:YouShouldCreateAtLeastTwoOptions"]);
        }
        
        NewEntity.PollOptions = new Collection<PollOptionDto>(PollOptions);
        return base.CreateEntityAsync();
    }

    protected override Task UpdateEntityAsync()
    {
        if (PollOptions.Count < 2)
        {
            return Message.Warn(L["Poll:YouShouldCreateAtLeastTwoOptions"]);
        }
        EditingEntity.PollOptions = new Collection<PollOptionDto>(PollOptions);
        return base.UpdateEntityAsync();
    }

    protected async override Task OpenEditModalAsync(PollDto entity)
    {
        await GetWidgetsAsync();
        await OnSelectedTabChangedAsync("question");
        await base.OpenEditModalAsync(entity);
        PollOptions = EditingEntity.PollOptions.ToList();
    }

    protected override Task CloseCreateModalAsync()
    {
        PollOptions.Clear();
        return base.CloseCreateModalAsync();
    }

    protected override Task CloseEditModalAsync()
    {
        PollOptions.Clear();
        return base.CloseEditModalAsync();
    }

    protected virtual Task OnSelectedTabChangedAsync(string tab)
    {
        SelectedTab = tab;
        return Task.CompletedTask;
    }

    protected virtual Task AddNewOptionsAsync()
    {
        if (NewOption.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        var option = new PollOptionDto
        {
            Text = NewOption,
            Order = PollOptions.Any() ? PollOptions.Max(x => x.Order) + 1 : 1
        };
        if (EditingEntity != null)
        {
            option.Id = GuidGenerator.Create();
        }
        PollOptions.Add(option);
        NewOption = string.Empty;

        return Task.CompletedTask;
    }

    protected virtual async Task ChangeOptionOrderAsync(PollOptionDto option, bool up)
    {
        if (up && option.Order != 1)
        {
            PollOptions.First(x => x.Order == option.Order - 1).Order++;
            option.Order--;
        }

        if (!up && option.Order != PollOptions.Max(x => x.Order))
        {
            PollOptions.First(x => x.Order == option.Order + 1).Order--;
            option.Order++;
        }

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task RemoveOptionAsync(PollOptionDto option)
    {
        foreach (var pollOptionDto in PollOptions.Where(x => x.Order > option.Order))
        {
            pollOptionDto.Order--;
        }

        PollOptions.Remove(option);

        await InvokeAsync(StateHasChanged);
    }
    
    protected virtual string GetVoteCountText(GetResultDto poll)
    {
        if (poll.PollVoteCount == 0)
        {
            return L["NoVotes"].Value;
        }
        
        if (poll.PollVoteCount == 1)
        {
            return L.GetString("SingleVoteCount");
        }
        
        return L.GetString("VoteCount{0}", poll.PollVoteCount);
    }
}