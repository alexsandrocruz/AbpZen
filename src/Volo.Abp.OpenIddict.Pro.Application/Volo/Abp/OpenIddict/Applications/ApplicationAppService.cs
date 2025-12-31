using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Abstractions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Permissions;

namespace Volo.Abp.OpenIddict.Applications;

[Authorize(AbpOpenIddictProPermissions.Application.Default)]
public class ApplicationAppService : AbpOpenIddictProAppService, IApplicationAppService
{
    protected IOpenIddictApplicationManager ApplicationManager { get; }
    protected IOpenIddictApplicationRepository ApplicationRepository { get; }

    public ApplicationAppService(IOpenIddictApplicationManager applicationManager, IOpenIddictApplicationRepository applicationRepository)
    {
        ApplicationManager = applicationManager;
        ApplicationRepository = applicationRepository;
    }

    public virtual async Task<ApplicationDto> GetAsync(Guid id)
    {
        var application = await ApplicationManager.FindByIdAsync(ConvertIdentifierToString(id))
                          ?? throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);

        return ObjectMapper.Map<OpenIddictApplicationModel, ApplicationDto>(application.As<OpenIddictApplicationModel>());
    }

    public virtual async Task<PagedResultDto<ApplicationDto>> GetListAsync(GetApplicationListInput input)
    {
        var apps = await ApplicationRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.Filter);

        var totalCount = await ApplicationRepository.GetCountAsync(input.Filter);

        var dtos = ObjectMapper.Map<List<OpenIddictApplicationModel>, List<ApplicationDto>>(apps.Select(x => x.ToModel()).ToList());

        return new PagedResultDto<ApplicationDto>(totalCount, dtos);
    }

    [Authorize(AbpOpenIddictProPermissions.Application.Create)]
    public virtual async Task<ApplicationDto> CreateAsync(CreateApplicationInput input)
    {
        await CheckInputDtoAsync(input);

        var descriptor = new AbpApplicationDescriptor
        {
            ApplicationType = input.ApplicationType,
            ClientId = input.ClientId,
            ClientSecret = input.ClientSecret,
            ConsentType = input.ConsentType,
            DisplayName = input.DisplayName,
            ClientType = input.ClientType,
            ClientUri = input.ClientUri,
            LogoUri = input.LogoUri
        };

        if (input.AllowHybridFlow)
        {
            input.AllowAuthorizationCodeFlow = true;
            input.AllowImplicitFlow = true;

            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
            if (string.Equals(input.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
        }

        if (input.AllowLogoutEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
        }

        if (input.AllowDeviceEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
        }

        if (input.AllowAuthorizationCodeFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
        }

        if (input.AllowClientCredentialsFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
        }

        if (input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
        }

        if (input.AllowPasswordFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
        }

        if (input.AllowRefreshTokenFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
        }

        if (input.AllowAuthorizationCodeFlow || input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
        }

        if (input.AllowAuthorizationCodeFlow || input.AllowClientCredentialsFlow || input.AllowPasswordFlow || input.AllowRefreshTokenFlow || input.AllowDeviceEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
        }

        if (input.AllowAuthorizationCodeFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
        }

        if (input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);

            if (string.Equals(input.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
            }
        }

        foreach (var uri in (input.RedirectUris ?? new HashSet<string>()).Select(x => new Uri(x, UriKind.Absolute)))
        {
            descriptor.RedirectUris.Add(uri);
        }

        foreach (var uri in (input.PostLogoutRedirectUris ?? new HashSet<string>()).Select(x => new Uri(x, UriKind.Absolute)))
        {
            descriptor.PostLogoutRedirectUris.Add(uri);
        }

        input.ExtensionGrantTypes ??= new HashSet<string>();
        input.ExtensionGrantTypes.RemoveAll(x => x == OpenIddictConstants.Permissions.GrantTypes.RefreshToken ||
                                                 x == OpenIddictConstants.Permissions.GrantTypes.Password ||
                                                 x == OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode ||
                                                 x == OpenIddictConstants.Permissions.GrantTypes.ClientCredentials ||
                                                 x == OpenIddictConstants.Permissions.GrantTypes.DeviceCode ||
                                                 x == OpenIddictConstants.Permissions.GrantTypes.Implicit);
        foreach (var grantType in input.ExtensionGrantTypes)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
        }

        foreach (var scope in (input.Scopes ?? new HashSet<string>()))
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
        }

        var application = await ApplicationManager.CreateAsync(descriptor);

        input.MapExtraPropertiesTo(application.As<OpenIddictApplicationModel>());

        await ApplicationManager.UpdateAsync(application);

        return ObjectMapper.Map<OpenIddictApplicationModel, ApplicationDto>(application.As<OpenIddictApplicationModel>());
    }

    [Authorize(AbpOpenIddictProPermissions.Application.Update)]
    public virtual async Task<ApplicationDto> UpdateAsync(Guid id, UpdateApplicationInput input)
    {
        var application = (await ApplicationManager.FindByIdAsync(ConvertIdentifierToString(id))).As<OpenIddictApplicationModel>()
            ?? throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);

        await CheckInputDtoAsync(input, application);

         var descriptor = new AbpApplicationDescriptor();
        await ApplicationManager.PopulateAsync(descriptor, application);

        descriptor.ApplicationType = input.ApplicationType;
        descriptor.ClientId = input.ClientId;
        descriptor.ConsentType = input.ConsentType;
        descriptor.DisplayName = input.DisplayName;
        descriptor.ClientType = input.ClientType;
        descriptor.ClientUri = input.ClientUri;
        descriptor.LogoUri = input.LogoUri;

        if (!string.IsNullOrEmpty(input.ClientSecret))
        {
            descriptor.ClientSecret = input.ClientSecret;
        }

        if (string.Equals(descriptor.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
        {
            descriptor.ClientSecret = null;
        }

        if (input.AllowHybridFlow)
        {
            input.AllowAuthorizationCodeFlow = true;
            input.AllowImplicitFlow = true;

            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
            if (string.Equals(input.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
            else
            {
                descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
            }
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
        }

        if (input.AllowDeviceEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Device);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
        }

        if (input.AllowAuthorizationCodeFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
        }

        if (input.AllowClientCredentialsFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
        }

        if (input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.Implicit);
        }

        if (input.AllowPasswordFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.Password);
        }

        if (input.AllowRefreshTokenFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
        }

        if (input.AllowAuthorizationCodeFlow || input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Authorization);
        }

        if (input.AllowAuthorizationCodeFlow || input.AllowClientCredentialsFlow || input.AllowPasswordFlow || input.AllowRefreshTokenFlow || input.AllowDeviceEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Token);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Revocation);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Introspection);
        }

        if (input.AllowAuthorizationCodeFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.Code);
        }

        if (input.AllowImplicitFlow)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);

            if (string.Equals(input.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
            }
            else
            {
                descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.Token);
            }
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.ResponseTypes.Token);
        }

        descriptor.RedirectUris.Clear();
        foreach (var uri in (input.RedirectUris ?? new HashSet<string>()).Select(x => new Uri(x, UriKind.Absolute)))
        {
            descriptor.RedirectUris.Add(uri);
        }

        descriptor.PostLogoutRedirectUris.Clear();
        foreach (var uri in (input.PostLogoutRedirectUris ?? new HashSet<string>()).Select(x => new Uri(x, UriKind.Absolute)))
        {
            descriptor.PostLogoutRedirectUris.Add(uri);
        }

        descriptor.Permissions.RemoveAll(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType) &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.Password &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.DeviceCode &&
                                              x != OpenIddictConstants.Permissions.GrantTypes.Implicit);
        foreach (var grantType in input.ExtensionGrantTypes ?? new HashSet<string>())
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.GrantType + grantType);
        }

        if (input.AllowLogoutEndpoint)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
        }
        else
        {
            descriptor.Permissions.Remove(OpenIddictConstants.Permissions.Endpoints.Logout);
        }

        descriptor.Permissions.RemoveWhere(permission => permission.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope));
        foreach (var scope in (input.Scopes ?? new HashSet<string>()))
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
        }

        input.MapExtraPropertiesTo(application.As<OpenIddictApplicationModel>());

        await ApplicationManager.UpdateAsync(application, descriptor);

        return ObjectMapper.Map<OpenIddictApplicationModel, ApplicationDto>(application);
    }

    [Authorize(AbpOpenIddictProPermissions.Application.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var application = await ApplicationManager.FindByIdAsync(ConvertIdentifierToString(id))
             ?? throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);

        await ApplicationManager.DeleteAsync(application);
    }

    [Authorize(AbpOpenIddictProPermissions.Application.Update)]
    public virtual async Task<ApplicationTokenLifetimeDto> GetTokenLifetimeAsync(Guid id)
    {
        var application = await ApplicationManager.FindByIdAsync(ConvertIdentifierToString(id))
                          ?? throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);

        var settings = await ApplicationManager.GetSettingsAsync(application);
        return new ApplicationTokenLifetimeDto
        {
            AccessTokenLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.AccessToken))?.TotalSeconds,
            AuthorizationCodeLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode))?.TotalSeconds,
            DeviceCodeLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.DeviceCode))?.TotalSeconds,
            IdentityTokenLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.IdentityToken))?.TotalSeconds,
            RefreshTokenLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.RefreshToken))?.TotalSeconds,
            UserCodeLifetime = (await GetTokenLifetimeOrNullAsync(settings, OpenIddictConstants.Settings.TokenLifetimes.UserCode))?.TotalSeconds
        };
    }

    protected virtual Task<TimeSpan?> GetTokenLifetimeOrNullAsync(ImmutableDictionary<string, string> settings, string tokenType)
    {
        if (settings.TryGetValue(tokenType, out string? setting) &&
            TimeSpan.TryParse(setting, CultureInfo.InvariantCulture, out var value))
        {
            return Task.FromResult<TimeSpan?>(value);
        }

        return Task.FromResult<TimeSpan?>(null);
    }

    [Authorize(AbpOpenIddictProPermissions.Application.Update)]
    public virtual async Task SetTokenLifetimeAsync(Guid id, ApplicationTokenLifetimeDto input)
    {
        var application = await ApplicationManager.FindByIdAsync(ConvertIdentifierToString(id))
                          ?? throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);

        var descriptor = new AbpApplicationDescriptor();
        await ApplicationManager.PopulateAsync(descriptor, application);

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.AccessToken);
        if (input.AccessTokenLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.AccessToken,  TimeSpan.FromSeconds(input.AccessTokenLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode);
        if (input.AuthorizationCodeLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode, TimeSpan.FromSeconds(input.AuthorizationCodeLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.DeviceCode);
        if (input.DeviceCodeLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.DeviceCode, TimeSpan.FromSeconds(input.DeviceCodeLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.IdentityToken);
        if (input.IdentityTokenLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.IdentityToken, TimeSpan.FromSeconds(input.IdentityTokenLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.RefreshToken);
        if (input.RefreshTokenLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.RefreshToken, TimeSpan.FromSeconds(input.RefreshTokenLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        descriptor.Settings.Remove(OpenIddictConstants.Settings.TokenLifetimes.UserCode);
        if (input.UserCodeLifetime.HasValue)
        {
            descriptor.Settings.Add(OpenIddictConstants.Settings.TokenLifetimes.UserCode, TimeSpan.FromSeconds(input.UserCodeLifetime.Value).ToString("c", CultureInfo.InvariantCulture));
        }

        await ApplicationManager.UpdateAsync(application, descriptor);
    }

    protected virtual async Task CheckInputDtoAsync(ApplicationCreateOrUpdateDtoBase dto, OpenIddictApplicationModel application = null)
    {
        if (!dto.RedirectUris.IsNullOrEmpty())
        {
            foreach (var url in dto.RedirectUris.Select(x => x))
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                {
                    throw new UserFriendlyException(L["InvalidRedirectUri", url]);
                }
            }
        }

        if (!dto.PostLogoutRedirectUris.IsNullOrEmpty())
        {
            foreach (var url in dto.PostLogoutRedirectUris.Select(x => x))
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                {
                    throw new UserFriendlyException(L["InvalidPostLogoutRedirectUri", url]);
                }
            }
        }

        if (!dto.ClientSecret.IsNullOrEmpty() && string.Equals(dto.ClientType, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
        {
            throw new UserFriendlyException(L["NoClientSecretCanBeSetForPublicApplications"]);
        }

        if (dto.ClientSecret.IsNullOrEmpty() && string.Equals(dto.ClientType, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
        {
            if (application == null || application.ClientSecret.IsNullOrEmpty())
            {
                throw new UserFriendlyException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
            }
        }

        if (!dto.ClientId.IsNullOrEmpty() && await ApplicationManager.FindByClientIdAsync(dto.ClientId) != null)
        {
            if (application == null || application.ClientId != dto.ClientId)
            {
                throw new UserFriendlyException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
            }
        }
    }
}
