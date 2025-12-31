using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Components;
using Volo.CmsKit.Public.Newsletters;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class NewslettersManagement : IDisposable
{
    [Inject]
    protected INewsletterRecordAdminAppService NewsletterRecordAdminAppService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; }

    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    protected PageToolbar Toolbar { get; } = new();

    protected GetNewsletterRecordsRequestInput GetListInput = new();

    protected List<string> NewsletterPreferences = new();

    protected IReadOnlyList<NewsletterRecordDto> NewsletterRecords = Array.Empty<NewsletterRecordDto>();

    protected TableColumnDictionary TableColumns = new TableColumnDictionary();

    protected EntityActionDictionary EntityActions = new EntityActionDictionary();

    protected List<TableColumn> NewslettersManagementTableColumns => TableColumns.Get<NewslettersManagement>();

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;

    protected int? TotalCount;

    protected Modal DetailModal;
    protected Modal EditPreferencesModal;

    protected NewsletterRecordWithDetailsDto SelectedNewsletterRecord = new();

    protected UpdatePreferenceInput EditInput = new();

    protected List<NewsletterPreferenceDetailsView> EditSelectedNewsletterPreferences = new();

    [Inject]
    protected NewslettersManagementState NewslettersManagementState { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        NewsletterPreferences = await NewsletterRecordAdminAppService.GetNewsletterPreferencesAsync();
        NewslettersManagementState.OnDataGridChanged += GetNewslettersAsync;
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

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["NewsletterUsers"]));
        return ValueTask.CompletedTask;
    }

    protected virtual async Task SearchNewslettersAsync()
    {
        CurrentPage = 1;

        await GetNewslettersAsync();
    }

    protected virtual async Task GetNewslettersAsync()
    {
        try
        {
            GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
            GetListInput.MaxResultCount = PageSize;
            if (GetListInput.Preference == NewsletterSearchViewModel.NullSelectionValue)
            {
                GetListInput.Preference = null;
            }
            var result = await NewsletterRecordAdminAppService.GetListAsync(GetListInput);
            NewsletterRecords = result.Items;
            TotalCount = (int?)result.TotalCount;
            await SetToolbarItemsAsync();

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<NewsletterRecordDto> e)
    {
        CurrentPage = e.Page;

        await GetNewslettersAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        if (NewsletterRecords.Any())
        {
            if (Toolbar.Contributors.Count == 1)
            {
                Toolbar.AddButton(L["ExportCSV"],
                    ExportCsvAsync,
                    IconName.Download);
            }
        }
        else
        {
            Toolbar.Contributors.Clear();
            Toolbar.AddComponent<NewslettersImportDropdownComponent>(requiredPolicyName: CmsKitProAdminPermissions.Newsletters.Import);
        }

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<NewslettersManagement>()
            .AddRange(new EntityAction[]
            {
                new EntityAction
                {
                    Text = L["Detail"],
                    Primary = true,
                    Clicked = async (data) =>  await OpenDetailModalAsync(data.As<NewsletterRecordDto>().Id)
                },
                new EntityAction
                {
                    Text = L["EditPreferences"],
                    Clicked = async (data) => await OpenEditPreferencesModalAsync(data.As<NewsletterRecordDto>().EmailAddress)
                }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        NewslettersManagementTableColumns
            .AddRange(new TableColumn[]
            {
                new TableColumn
                {
                    Title = L["Detail"],
                    Actions = EntityActions.Get<NewslettersManagement>()
                },
                new TableColumn
                {
                    Title = L["EmailAddress"],
                    Data = nameof(NewsletterRecordDto.EmailAddress)
                },
                new TableColumn()
                {
                    Title = L["Preferences"],
                    Data = nameof(NewsletterRecordDto.Preferences),
                    ValueConverter = (value) => value.As<NewsletterRecordDto>().Preferences.JoinAsString(", ")
                },
                new TableColumn
                {
                    Title = L["CreationTime"],
                    Data = nameof(NewsletterRecordDto.CreationTime)
                }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual async Task ExportCsvAsync()
    {
        try
        {
            var downloadToken = await NewsletterRecordAdminAppService.GetDownloadTokenAsync();
            var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(CmsKitAdminRemoteServiceConsts.RemoteServiceName);
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }

            NavigationManager.NavigateTo(baseUrl + $"api/cms-kit-admin/newsletter/export-csv?token={downloadToken.Token}{culture}&Preference=" + GetListInput.Preference + "&Source=" + GetListInput.Source,forceLoad:true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected virtual async  Task EditPreferencesAsync()
    {
        try
        {
            EditInput.PreferenceDetails = EditSelectedNewsletterPreferences.Select(x => new PreferenceDetailsDto
            {
                Preference = x.Preference,
                IsEnabled = x.IsSelected
            }).ToList();
            await NewsletterRecordAdminAppService.UpdatePreferencesAsync(EditInput);
            await CloseEditPreferencesModalAsync();
            await GetNewslettersAsync();
            await Notify.Success("SavedSuccessfully");
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual async Task OpenDetailModalAsync(Guid id)
    {
        try
        {
            SelectedNewsletterRecord = await NewsletterRecordAdminAppService.GetAsync(id);
            await InvokeAsync(DetailModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual async Task OpenEditPreferencesModalAsync(string emailAddress)
    {
        try
        {
            var newsletterPreferences = await NewsletterRecordAdminAppService.GetNewsletterPreferencesAsync(emailAddress);

            EditSelectedNewsletterPreferences = newsletterPreferences.Select(x => new NewsletterPreferenceDetailsView
            {
                Preference = x.Preference,
                DisplayPreference = x.DisplayPreference,
                IsSelected = x.IsSelectedByEmailAddress
            }).ToList();

            EditInput = new UpdatePreferenceInput {
                EmailAddress = emailAddress,
                Source = "Admin",
                SourceUrl = "/CmsKit/Newsletters/EditPreferences"
            };

            await InvokeAsync(EditPreferencesModal.Show);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual Task SelectedAllChanged()
    {
        if(EditSelectedNewsletterPreferences.All(x => x.IsSelected))
        {
            EditSelectedNewsletterPreferences.ForEach(x => x.IsSelected = false);
        }
        else
        {
            EditSelectedNewsletterPreferences.ForEach(x => x.IsSelected = true);
        }

        return Task.CompletedTask;
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseDetailModalAsync()
    {
        return InvokeAsync(DetailModal.Hide);
    }

    protected virtual Task CloseEditPreferencesModalAsync()
    {
        EditInput = new UpdatePreferenceInput();
        return InvokeAsync(EditPreferencesModal.Hide);
    }

    public void Dispose()
    {
        NewslettersManagementState.OnDataGridChanged -= GetNewslettersAsync;
    }
}

public class NewsletterPreferenceDetailsView
{
    public string Preference { get; set; }

    public string DisplayPreference { get; set; }

    public bool IsSelected { get; set; }
}

public class NewsletterSearchViewModel
{
    public const string NullSelectionValue = "empty_value";
}
