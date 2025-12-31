using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class TagManagement
{
    protected List<TableColumn> TagsManagementTableColumns => TableColumns.Get<TagManagement>();
    protected PageToolbar Toolbar { get; } = new();
    
    public List<TagDefinitionDto> TagDefinitions { get; set; }
    
    public TagManagement()
    {
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
        LocalizationResource = typeof(CmsKitResource);

        CreatePolicyName = CmsKitAdminPermissions.Tags.Create;
        UpdatePolicyName = CmsKitAdminPermissions.Tags.Update;
        DeletePolicyName = CmsKitAdminPermissions.Tags.Delete;
    }

    protected async override Task OpenCreateModalAsync()
    {
        TagDefinitions = await AppService.GetTagDefinitionsAsync();
        await base.OpenCreateModalAsync();
        NewEntity.EntityType = TagDefinitions.FirstOrDefault()?.EntityType;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Tags"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(TagDto entity)
    {
        return string.Format(L["TagDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewTag"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
    
    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TagManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<TagDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<TagDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<TagDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        TagsManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Details"],
                        Actions = EntityActions.Get<TagManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["EntityType"],
                        Data = nameof(TagDto.EntityType)
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Data = nameof(TagDto.Name)
                    },
            });

        return base.SetTableColumnsAsync();
    }
}