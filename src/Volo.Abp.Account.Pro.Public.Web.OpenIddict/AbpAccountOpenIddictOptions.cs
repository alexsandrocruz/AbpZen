using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Account.Web;

public class AbpAccountOpenIddictOptions
{
    public string ImpersonationAuthenticationScheme { get; set; }

    public string LinkLoginAuthenticationScheme { get; set; }

    public bool IsTenantMultiDomain { get; set; }

    public Func<HttpContext, BasicTenantInfo, Task<string>> GetTenantDomain { get; set; }

    public Dictionary<string, string> ClientIdToDeviceMap { get; }

    public AbpAccountOpenIddictOptions()
    {
        ImpersonationAuthenticationScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        LinkLoginAuthenticationScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

        GetTenantDomain = (context, _) => Task.FromResult(context.Request.Scheme + "://" + context.Request.Host);
        ClientIdToDeviceMap = new Dictionary<string, string>();
    }
}
