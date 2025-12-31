using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise.Markdown;
using Microsoft.AspNetCore.Components;
using Volo.Abp.BlazoriseUI;
using Volo.CmsKit.Admin.GlobalResources;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public partial class GlobalResourceManagement
{
    [Inject]
    protected IGlobalResourceAdminAppService GlobalResourceAdminAppService { get; set; }
    
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);
    
    protected string[] HideIcons = new[] {"bold"};

    protected GlobalResourcesDto GlobalResources = new GlobalResourcesDto() {
        ScriptContent = string.Empty
    };

    protected string SelectedTab = "script";

    protected Markdown ScriptMarkdownRef;
    
    protected async override Task OnInitializedAsync()
    {
        await GetGlobalResourcesAsync();
        await InitializeScriptMarkdownAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual async Task InitializeScriptMarkdownAsync()
    {
        var initialized = false;
        while (!initialized)
        {
            if (ScriptMarkdownRef == null)
            {
                continue;
            }
            if (await ScriptMarkdownRef.GetValueAsync() == GlobalResources.ScriptContent)
            {
                initialized = true;
                continue;
            }

            if (await ScriptMarkdownRef.GetValueAsync() != string.Empty)
            {
                continue;
            }

            await ScriptMarkdownRef.SetValueAsync(GlobalResources.ScriptContent);
            initialized = true;
        }
    }

    protected virtual async Task GetGlobalResourcesAsync()
    {
        GlobalResources = await GlobalResourceAdminAppService.GetAsync();
    }
    
    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:CMS"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["GlobalResources"]));
        return ValueTask.CompletedTask;
    }

    protected virtual async Task UpdateGlobalResourcesAsync()
    {
        try
        {
            await GlobalResourceAdminAppService.SetGlobalResourcesAsync(new GlobalResourcesUpdateDto 
            {
                Script = GlobalResources.ScriptContent, Style = GlobalResources.StyleContent
            });
            
            await Message.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }
}