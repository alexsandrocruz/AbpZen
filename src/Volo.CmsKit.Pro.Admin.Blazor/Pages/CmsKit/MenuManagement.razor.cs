using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class MenuManagement
{
    [Inject]
    protected IMenuItemAdminAppService MenuItemAdminAppService { get; set; }
    
    [Inject]
    protected IPageAdminAppService PageAdminAppService { get; set; }

    [Inject]
    protected IFeatureChecker FeatureChecker { get; set; }

    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
    
    protected PageToolbar Toolbar { get; } = new();
    
    protected List<MenuItemTreeViewModel> MenuTrees { get; set; } = new();

    protected ListResultDto<MenuItemDto> MenuItems { get; set; } = new();

    protected bool IsPageFeatureEnabled { get; set; }

    protected bool HasCreatePermission { get; set; }
    
    protected bool HasUpdatePermission { get; set; }
    
    protected bool HasDeletePermission { get; set; }

    protected MenuItemCreateViewModel NewMenuItem { get; set; } = new();
    
    protected Guid EditingMenuItemId { get; set; }
    
    protected MenuItemUpdateViewModel EditingMenuItem { get; set; } = new();
    
    protected IReadOnlyList<PageDto> Pages { get; set; } = Array.Empty<PageDto>();

    protected string SelectedTab = "url";

    protected Modal CreateModal;

    protected Modal EditModal;

    protected Validations CreateValidationsRef;

    protected Validations EditValidationsRef;

    protected async override Task OnInitializedAsync()
    {
        await CheckIsPageFeatureEnabledAsync();
        await SetBreadcrumbItemsAsync();
        await SetToolbarItemsAsync();
        await SetPermissionsAsync();
        await GetMenuTreesAsync();
    }

    protected virtual async Task GetMenuTreesAsync()
    {
        MenuItems = await MenuItemAdminAppService.GetListAsync();
        MenuTrees = MenuItems.Items.Select(x => new MenuItemTreeViewModel(x.Id, x.DisplayName, x.ParentId, x.Icon)).ToList();

        var childHash = MenuTrees.ToLookup(cat => cat.ParentId);
        foreach (var menu in MenuTrees)
        {
            menu.Children = childHash[menu.Id].ToList();
        }

        MenuTrees.RemoveAll(x => x.ParentId != null);
    }

    protected virtual async Task CheckIsPageFeatureEnabledAsync()
    {
        IsPageFeatureEnabled = GlobalFeatureManager.Instance.IsEnabled<PagesFeature>()
            && await FeatureChecker.IsEnabledAsync(CmsKitFeatures.PageEnable);
    }

    protected virtual async Task GetPagesAsync()
    {
        if (IsPageFeatureEnabled)
        {
            Pages = (await PageAdminAppService.GetListAsync(new GetPagesInputDto{MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount})).Items;
        }
    }

    protected virtual async ValueTask SetPermissionsAsync()
    {
        HasCreatePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Menus.Create);
        HasUpdatePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Menus.Update);
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(CmsKitAdminPermissions.Menus.Delete);
    }
    
    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["MenuItems"]));
        return ValueTask.CompletedTask;
    }
    
    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewMenuItem"],
            () => OpenCreatePageAsync(),
            IconName.Add,
            requiredPolicyName: CmsKitAdminPermissions.Menus.Create);

        return ValueTask.CompletedTask;
    }
    
    protected virtual async Task DeleteMenuItemAsync(MenuItemTreeViewModel menuItem)
    {
        if (await Message.Confirm(L["MenuItemDeletionConfirmationMessage", menuItem.DisplayName]))
        {
            await MenuItemAdminAppService.DeleteAsync(menuItem.Id);
            
            if(MenuTrees.Contains(menuItem))
            {
                MenuTrees.Remove(menuItem);
            }
            else
            {
                foreach (var menu in MenuTrees)
                {
                    if (menu.RemoveMenuItem(menuItem))
                    {
                        break;
                    }
                }
            }

            await InvokeAsync(StateHasChanged);
        }
    }
    
    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }
    
    protected virtual Task CloseCreateModalAsync()
    {
        return InvokeAsync(CreateModal.Hide);
    }
    
    protected virtual Task CloseEditModalAsync()
    {
        return InvokeAsync(EditModal.Hide);
    }

    protected virtual Task OnSelectedTabChangedAsync(string name)
    {
        SelectedTab = name;
        return Task.CompletedTask;
    }

    protected virtual async Task OpenCreatePageAsync(Guid? parentId = null)
    {
        try
        {
            if (CreateValidationsRef != null)
            {
                await CreateValidationsRef.ClearAll();
            }
            
            await OnSelectedTabChangedAsync("url");
            
            await GetPagesAsync();

            await AuthorizationService.CheckAsync(CmsKitAdminPermissions.Menus.Create);

            NewMenuItem = new MenuItemCreateViewModel
            {
                ParentId = parentId
            };

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (CreateModal != null)
                {
                    await CreateModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task OpenUpdatePageAsync(Guid id)
    {
        try
        {
            if (CreateValidationsRef != null)
            {
                await CreateValidationsRef.ClearAll();
            }
            
            await OnSelectedTabChangedAsync("url");
            
            await GetPagesAsync();

            await AuthorizationService.CheckAsync(CmsKitAdminPermissions.Menus.Update);

            EditingMenuItemId = id;
            EditingMenuItem = ObjectMapper.Map<MenuItemWithDetailsDto, MenuItemUpdateViewModel>(await MenuItemAdminAppService.GetAsync(id));

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (EditModal != null)
                {
                    await EditModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task CreateMenuItemAsync()
    {
        try
        {
            if (!await CreateValidationsRef.ValidateAll())
            {
                return;
            }
            var menuItemCreateInput = ObjectMapper.Map<MenuItemCreateViewModel, MenuItemCreateInput>(NewMenuItem);
            var menuItem = await MenuItemAdminAppService.CreateAsync(menuItemCreateInput);
            var menuItemNode = new MenuItemTreeViewModel(menuItem.Id,menuItem.DisplayName, menuItem.ParentId, menuItem.Icon);

            if (menuItemNode.ParentId == null)
            {
                MenuTrees.Add(menuItemNode);
            }
            else
            {
                foreach (var menu in MenuTrees)
                {
                    if (menu.AddChild(menuItemNode))
                    {
                        break;
                    }
                }
            }
            
            await CloseCreateModalAsync();
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual async Task UpdateMenuItemAsync()
    {
        try
        {
            if (!await EditValidationsRef.ValidateAll())
            {
                return;
            }

            var menuItemUpdateInput = ObjectMapper.Map<MenuItemUpdateViewModel, MenuItemUpdateInput>(EditingMenuItem);
            var menuItem = await MenuItemAdminAppService.UpdateAsync(EditingMenuItemId, menuItemUpdateInput);
            var menuItemNode = new MenuItemTreeViewModel(menuItem.Id,menuItem.DisplayName, menuItem.ParentId, menuItem.Icon);
 
            foreach (var menu in MenuTrees)
            {
                if (menu.Update(menuItemNode))
                {
                    break;
                }
            }

            await CloseEditModalAsync();
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    public class MenuItemTreeViewModel
    {
        public Guid Id { get; set; }
        
        public Guid? ParentId { get; set; }

        public string DisplayName { get; set; }
        
        public bool HasChildren => Children.Any();
        
        public string Icon { get; set; }

        public List<MenuItemTreeViewModel> Children { get; set; }

        public MenuItemTreeViewModel(Guid id, string name, Guid? parentId, string icon)
        {
            Id = id;
            DisplayName = name;
            ParentId = parentId;
            Children = new List<MenuItemTreeViewModel>();
            Icon = icon.IsNullOrWhiteSpace() ? "fas fa-folder fs-15px text-primary me-1" : icon;
        }

        public bool Update(MenuItemTreeViewModel menuItem)
        {
            if (Id == menuItem.Id)
            {
                DisplayName = menuItem.DisplayName;
                Icon = menuItem.Icon;
                return true;
            }

            return Children.Any(item => item.Update(menuItem));
        }

        public bool AddChild(MenuItemTreeViewModel child)
        {
            if (Id == child.ParentId)
            {
                Children.Add(child);
                return true;
            }

            return Children.Any(item => item.AddChild(child));
        }
        
        public bool RemoveMenuItem(MenuItemTreeViewModel menuItem)
        {
            if (Children.Contains(menuItem))
            {
                Children.Remove(menuItem);
                return true;
            }

            return Children.Any(item => item.RemoveMenuItem(menuItem));
        }
    }

    public class MenuItemCreateViewModel : ExtensibleObject
    {
        public Guid? ParentId { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string Url { get; set; }

        public Guid? PageId { get; set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

    }

    public class MenuItemUpdateViewModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [Required]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Url { get; set; }

        public string Icon { get; set; }

        public string Target { get; set; }

        public string ElementId { get; set; }

        public string CssClass { get; set; }

        public Guid? PageId { get; set; }

        public string? PageTitle { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}