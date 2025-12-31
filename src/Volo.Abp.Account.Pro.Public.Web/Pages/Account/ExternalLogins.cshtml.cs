using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.Reflection;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

public class ExternalLoginsModel : AccountPageModel
{
    protected readonly IAuthenticationSchemeProvider SchemeProvider;
    protected readonly IAccountExternalLoginAppService AccountExternalLoginAppService;
    protected readonly IAccountExternalProviderAppService AccountExternalProviderAppService;
    protected readonly IOptions<AbpAccountOptions> AccountOptions;

    public List<ExternalProviderModel> VisibleExternalProviders { get; set; }

    public ExternalLoginsModel(IAuthenticationSchemeProvider schemeProvider,
        IAccountExternalLoginAppService accountExternalLoginAppService,
        IAccountExternalProviderAppService accountExternalProviderAppService,
        IOptions<AbpAccountOptions> accountOptions)
    {
        SchemeProvider = schemeProvider;
        AccountExternalLoginAppService = accountExternalLoginAppService;
        AccountExternalProviderAppService = accountExternalProviderAppService;
        AccountOptions = accountOptions;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        VisibleExternalProviders = (await GetExternalProviders()).Where(x => !string.IsNullOrWhiteSpace(x.DisplayName)).ToList();

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    protected virtual async Task<List<ExternalProviderModel>> GetExternalProviders()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var externalProviders = await AccountExternalProviderAppService.GetAllAsync();

        var externalProviderModels = new List<ExternalProviderModel>();
        foreach (var scheme in schemes)
        {
            if (IsRemoteAuthenticationHandler(scheme, externalProviders) || scheme.Name.Equals(AccountOptions.Value.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
            {
                externalProviderModels.Add(new ExternalProviderModel
                {
                    DisplayName = scheme.DisplayName,
                    AuthenticationScheme = scheme.Name,
                    Icon = AccountOptions.Value.ExternalProviderIconMap.GetOrDefault(scheme.Name)
                });
            }
        }

        var externalLogins = await AccountExternalLoginAppService.GetListAsync();
        externalProviderModels.RemoveAll(x => externalLogins.Any(y => y.LoginProvider == x.AuthenticationScheme));

        return externalProviderModels;
    }

    protected virtual bool IsRemoteAuthenticationHandler(AuthenticationScheme scheme, ExternalProviderDto externalProviders)
    {
        if (ReflectionHelper.IsAssignableToGenericType(scheme.HandlerType, typeof(RemoteAuthenticationHandler<>)))
        {
            var provider = externalProviders.Providers.FirstOrDefault(x => x.Name == scheme.Name);
            return provider == null || provider.Enabled;
        }

        return false;
    }

    public class ExternalProviderModel
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }

        public string Icon { get; set; }
    }
}
