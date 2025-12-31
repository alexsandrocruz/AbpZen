using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

namespace Volo.CmsKit.Pro.Admin.Blazor.WebAssembly.Pages.CmsKit;

[ExposeServices(typeof(BlogPostUpdate))]
public class WebAssemblyBlogPostUpdate : BlogPostUpdate
{
    [Inject]
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; set; }

    protected async override Task<string> GetCoverImageUrlAsync()
    {
        var url = "/api/cms-kit/media/" + BlogPost.CoverImageMediaId;
        return (await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync(CmsKitAdminRemoteServiceConsts.RemoteServiceName))?.BaseUrl + url;
    }
}
