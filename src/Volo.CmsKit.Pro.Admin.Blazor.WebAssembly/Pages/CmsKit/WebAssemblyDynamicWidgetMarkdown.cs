using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

namespace Volo.CmsKit.Pro.Admin.Blazor.WebAssembly.Pages.CmsKit;

[ExposeServices(typeof(DynamicWidgetMarkdown))]
public class WebAssemblyDynamicWidgetMarkdown : DynamicWidgetMarkdown
{
    [Inject]
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; set; }

    protected async override Task<string> GetUploadUrlAsync(string url)
    {
        return (await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync(CmsKitAdminRemoteServiceConsts.RemoteServiceName))?.BaseUrl + url;
    }
}
