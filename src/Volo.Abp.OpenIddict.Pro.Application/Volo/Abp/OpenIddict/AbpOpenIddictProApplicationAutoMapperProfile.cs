using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict;

public class AbpOpenIddictProApplicationAutoMapperProfile : Profile
{
    public AbpOpenIddictProApplicationAutoMapperProfile()
    {
        CreateMap<OpenIddictApplicationModel, ApplicationDto>(MemberList.Destination)
            .MapExtraProperties()
            .ForMember(d => d.ClientSecret, x => x.Ignore())
            .ForMember(d => d.AllowAuthorizationCodeFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode)))
            .ForMember(d => d.AllowClientCredentialsFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials)))
            .ForMember(d => d.AllowImplicitFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.Implicit)))
            .ForMember(d => d.AllowHybridFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode) && s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.Implicit)))
            .ForMember(d => d.AllowPasswordFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.Password)))
            .ForMember(d => d.AllowRefreshTokenFlow, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.RefreshToken)))
            .ForMember(d => d.AllowLogoutEndpoint, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.Endpoints.Logout)))
            .ForMember(d => d.AllowDeviceEndpoint, x => x.MapFrom(s => s.Permissions.Contains(OpenIddictConstants.Permissions.Endpoints.Device)))
            .ForMember(d => d.RedirectUris, x => x.MapFrom(s => JsonStringToHashSet(s.RedirectUris)))
            .ForMember(d => d.PostLogoutRedirectUris, x => x.MapFrom(s => JsonStringToHashSet(s.PostLogoutRedirectUris)))
            .ForMember(d => d.ExtensionGrantTypes, x => x.MapFrom(s => JsonStringToHashSet(s.Permissions)
                .Where(x => x != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                                  x != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                                  x != OpenIddictConstants.Permissions.GrantTypes.Implicit &&
                                  x != OpenIddictConstants.Permissions.GrantTypes.Password &&
                                  x != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                                  x != OpenIddictConstants.Permissions.GrantTypes.DeviceCode)
                .Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType)).Select(p => p.Substring(OpenIddictConstants.Permissions.Prefixes.GrantType.Length))))
            .ForMember(d => d.Scopes, x => x.MapFrom(s => JsonStringToHashSet(s.Permissions)
                .Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope)).Select(p => p.Substring(OpenIddictConstants.Permissions.Prefixes.Scope.Length))));

        CreateMap<OpenIddictScopeModel, ScopeDto>(MemberList.Destination)
            .MapExtraProperties()
            .ForMember(d => d.BuildIn, x => x.Ignore())
            .ForMember(d => d.Resources, x => x.MapFrom(s => JsonStringToHashSet(s.Resources)));
    }

    private HashSet<string> JsonStringToHashSet(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return ImmutableArray.Create<string>().ToHashSet();
        }

        using (var document = JsonDocument.Parse(json))
        {
            var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var value = element.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder.Add(value);
            }

            return builder.ToImmutable().ToHashSet();
        }
    }
}
