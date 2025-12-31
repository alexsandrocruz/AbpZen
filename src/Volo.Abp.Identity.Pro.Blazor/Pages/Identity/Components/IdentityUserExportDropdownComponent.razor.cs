using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Json;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

public partial class IdentityUserExportDropdownComponent
{
    protected const string ExportToExcelEndpoint = "api/identity/users/export-as-excel";
    protected const string ExportToCsvEndpoint = "api/identity/users/export-as-csv";
    
    [Inject]
    protected IIdentityUserAppService IdentityUserAppService { get; set; }
    
    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    [Inject]
    protected IJsonSerializer JsonSerializer { get; set; }
    
    [Inject]
    protected UserManagementState UserManagementState { get; set; }

    protected virtual async Task ExportToExcelFileAsync()
    {
        await ExportToFileAsync(ExportToExcelEndpoint);
    }

    protected virtual async Task ExportToCsvFileAsync()
    {
        await ExportToFileAsync(ExportToCsvEndpoint);
    }
    
    private  async Task ExportToFileAsync(string endpoint)
    {
        var filterJsonStr = JsonSerializer.Serialize( UserManagementState.GetFilter());
        var queryString = JsonSerializer.Deserialize<Dictionary<string, object>>(filterJsonStr).Select(x => x.Key+ "=" + x.Value).JoinAsString("&");
        
        var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentityProRemoteServiceConsts.RemoteServiceName);
        var downloadToken = await IdentityUserAppService.GetDownloadTokenAsync();
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if(!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }
        NavigationManager.NavigateTo($"{baseUrl}{endpoint}?token={downloadToken.Token}&{queryString}{culture}", forceLoad: true);
    }
}