using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Public.UrlShorting;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;
using ShortenedUrlDto = Volo.CmsKit.Admin.UrlShorting.ShortenedUrlDto;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class UrlForwardingManagement
{
    protected List<TableColumn> UrlForwardingManagementTableColumns => TableColumns.Get<UrlForwardingManagement>();

    protected PageToolbar Toolbar { get; } = new();

    protected string EditingSource { get; set; }

    protected bool EditingIsRegex { get; set; }

    protected string RegexTestInput { get; set; }

    protected string RegexTestOutput { get; set; }

    protected string RegexOutputCssClass { get; set; }

    [Inject]
    protected IOptions<UrlShortingOptions> Options { get; set; }

    public UrlForwardingManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitProAdminPermissions.UrlShorting.Create;
        UpdatePolicyName = CmsKitProAdminPermissions.UrlShorting.Update;
        DeletePolicyName = CmsKitProAdminPermissions.UrlShorting.Delete;
    }

    protected override Task OpenEditModalAsync(ShortenedUrlDto entity)
    {
        EditingSource = entity.Source;
        EditingIsRegex = entity.IsRegex;
        RegexTestInput = null;
        RegexTestOutput = null;
        return base.OpenEditModalAsync(entity);
    }

    protected override Task OpenCreateModalAsync()
    {
        RegexTestInput = null;
        RegexTestOutput = null;
        return base.OpenCreateModalAsync();
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["UrlForwarding"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(ShortenedUrlDto entity)
    {
        return string.Format(L["ForwardedUrlDeletionConfirmationMessage"]);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["ForwardaUrl"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<UrlForwardingManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<ShortenedUrlDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<ShortenedUrlDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<ShortenedUrlDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        UrlForwardingManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<UrlForwardingManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["Source"],
                        Data = nameof(ShortenedUrlDto.Source),
                        Sortable = true
                    },
                    new TableColumn
                    {
                        Title = L["Target"],
                        Data = nameof(ShortenedUrlDto.Target),
                        Sortable = true
                    },
            });

        return base.SetTableColumnsAsync();
    }

    protected virtual Task OnNewEntitySourceChangedAsync(string value)
    {
        NewEntity.Source = value;
        return TestRegexAsync(true);
    }

    protected virtual Task OnNewEntityTargetChangedAsync(string value)
    {
        NewEntity.Target = value;
        return TestRegexAsync(true);
    }

    protected virtual Task OnEditingTargetChangedAsync(string value)
    {
        EditingEntity.Target = value;
        return TestRegexAsync(false);
    }

    protected virtual Task OnRegexTestInputChangedAsync(string value, bool isCreate)
    {
        RegexTestInput = value;

        return TestRegexAsync(isCreate);
    }

    protected virtual Task TestRegexAsync(bool isCreate)
    {
        if ((isCreate && !NewEntity.IsRegex) || (!isCreate && !EditingIsRegex) || RegexTestInput.IsNullOrWhiteSpace())
        {
            return Task.CompletedTask;
        }

        var source = isCreate ? NewEntity.Source : EditingSource;
        var target = isCreate ? NewEntity.Target : EditingEntity.Target;

        var result = Options.Value.RegexIgnoreCase ? Regex.Match(RegexTestInput, source, RegexOptions.IgnoreCase) : Regex.Match(RegexTestInput, source);

        if (!result.Success)
        {
            RegexTestOutput = L["NoMatch"];
            RegexOutputCssClass = "text-danger";
            return Task.CompletedTask;
        }

        if (target.IsNullOrWhiteSpace())
        {
            RegexTestOutput = string.Empty;
            RegexOutputCssClass = "d-none";
            return Task.CompletedTask;
        }

        RegexTestOutput = result.Result(target);
        RegexOutputCssClass = string.Empty;

        return Task.CompletedTask;
    }
}