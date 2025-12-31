using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityProRemoteServiceConsts.ModuleName)]
[ControllerName("ExternalLogin")]
[Route("api/identity/external-login")]
public class IdentityExternalLoginController : AbpControllerBase, IIdentityExternalLoginAppService
{
    public IIdentityExternalLoginAppService ExternalLoginAppService { get; }

    public IdentityExternalLoginController(IIdentityExternalLoginAppService externalLoginAppService)
    {
        ExternalLoginAppService = externalLoginAppService;
    }
    
    [HttpPost]
    public virtual Task CreateOrUpdateAsync()
    {
        return ExternalLoginAppService.CreateOrUpdateAsync();
    }
}