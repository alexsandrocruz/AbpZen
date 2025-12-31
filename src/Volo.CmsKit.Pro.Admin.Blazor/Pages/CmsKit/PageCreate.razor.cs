using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.CmsKit.Admin.MediaDescriptors;
using Volo.CmsKit.Admin.Pages;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class PageCreate
{
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
    
    [Inject]
    protected IPageAdminAppService PageAdminAppService { get; set; }
    
    [Inject]
    protected IMediaDescriptorAdminAppService MediaDescriptorAdminAppService { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    protected CreatePageInputDto Page { get; set; } = new CreatePageInputDto();

    protected Validations ValidationsRef;

    protected string[] HideIcons = new[] {"bold"};

    protected string SelectedTab = "content";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  
    
    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Pages"]));
        return ValueTask.CompletedTask;
    }
    protected virtual async Task CreatePageAsync()
    {
        try
        {
            if (await ValidationsRef.ValidateAll())
            {
                await PageAdminAppService.CreateAsync(Page);
                NavigationManager.NavigateTo("cms/pages");
            }
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }
}