using Blazorise;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.LanguageManagement.Blazor.Pages.LanguageManagement.Components;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.LanguageManagement.Blazor.Pages.LanguageManagement;

public partial class LanguageManagement
{
    protected bool HasChangeDefaultPermission;
    protected string ChangeDefaultPolicyName;
    protected List<CultureInfoDto> Cultures = new List<CultureInfoDto>(0);
    protected PageToolbar Toolbar { get; } = new();
    private List<TableColumn> LanguageManagementTableColumns => TableColumns.Get<LanguageManagement>();

    public LanguageManagement()
    {
        ObjectMapperContext = typeof(LanguageManagementBlazorModule);
        LocalizationResource = typeof(LanguageManagementResource);

        CreatePolicyName = LanguageManagementPermissions.Languages.Create;
        UpdatePolicyName = LanguageManagementPermissions.Languages.Edit;
        DeletePolicyName = LanguageManagementPermissions.Languages.Delete;
        ChangeDefaultPolicyName = LanguageManagementPermissions.Languages.ChangeDefault;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        HasChangeDefaultPermission = await AuthorizationService.IsGrantedAsync(ChangeDefaultPolicyName);

        Cultures = await GetCulturesAsync();
    }

    protected override string GetDeleteConfirmationMessage(LanguageDto entity)
    {
        return string.Format(entity.IsDefaultLanguage ? L["DefaultLanguageDeletionConfirmationMessage"] : L["LanguageDeletionConfirmationMessage"], entity.DisplayName);
    }

    protected virtual async Task<List<CultureInfoDto>> GetCulturesAsync()
    {
        var cultures = await AppService.GetCulturelistAsync();
        if (cultures.Any())
        {
            var firstCulture = cultures.First();
            firstCulture.DisplayName = L["NotAssigned"];
            firstCulture.Name = "empty_value";
        }

        return cultures;
    }

    protected virtual Task OnNewLanguageCultureNameChangedAsync(string value)
    {
        NewEntity.CultureName = value;
        NewEntity.UiCultureName = value;
        NewEntity.DisplayName = value == "empty_value" ? null : value;

        return Task.CompletedTask;
    }

    protected virtual Task ValidateCultureNameAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var value = Convert.ToString(e.Value);
        e.Status = string.IsNullOrEmpty(value) || value == "empty_value"
            ? ValidationStatus.Error
            : ValidationStatus.Success;
        
        return Task.CompletedTask;
    }

    protected virtual async Task SetAsDefaultAsync(LanguageDto entity)
    {
        try
        {
            await AuthorizationService.CheckAsync(ChangeDefaultPolicyName);
            await AppService.SetAsDefaultAsync(entity.Id);
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["LanguageManagement"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Languages"]));

        return base.SetBreadcrumbItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<LanguageManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<LanguageDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<LanguageDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<LanguageDto>())
                    },
                    new EntityAction
                    {
                        Text = L["SetAsDefaultLanguage"],
                        Visible = (data) => HasChangeDefaultPermission,
                        Clicked = async (data) => await SetAsDefaultAsync(data.As<LanguageDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        LanguageManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<LanguageManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName"],
                        Data = nameof(LanguageDto.DisplayName),
                        Sortable = true,
                        Component = typeof(LanguageNameColumnComponent)
                    },
                    new TableColumn
                    {
                        Title = L["CultureName"],
                        Data = nameof(LanguageDto.CultureName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["UiCultureName"],
                        Data = nameof(LanguageDto.UiCultureName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["IsEnabled"],
                        Data = nameof(LanguageDto.IsEnabled),
                        Sortable = true,
                        Component = typeof(IsEnabledColumnComponent)
                    }
            });

        LanguageManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(LanguageManagementModuleExtensionConsts.ModuleName,
            LanguageManagementModuleExtensionConsts.EntityNames.Language));

        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["CreateNewLanguage"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
}
