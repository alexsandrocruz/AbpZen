using AutoMapper;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Pro.Blazor.Pages;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Blazor;

public class AbpOpenIddictProBlazorAutoMapperProfile : Profile
{
    public AbpOpenIddictProBlazorAutoMapperProfile()
    {
        CreateMap<ScopeModalView, CreateScopeInput>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ScopeDto, UpdateScopeInput>()
            .MapExtraProperties();

        CreateMap<ScopeModalView, UpdateScopeInput>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ScopeDto, ScopeModalView>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ApplicationModalView, CreateApplicationInput>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => new HashSetStringConverter(f => f != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                f != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                f != OpenIddictConstants.Permissions.GrantTypes.Implicit &&
                f != OpenIddictConstants.Permissions.GrantTypes.Password &&
                f != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                f != OpenIddictConstants.Permissions.GrantTypes.DeviceCode).Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));

        CreateMap<ApplicationDto, UpdateApplicationInput>()
            .MapExtraProperties();

        CreateMap<ApplicationModalView, UpdateApplicationInput>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => new HashSetStringConverter(f => f != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                f != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                f != OpenIddictConstants.Permissions.GrantTypes.Implicit &&
                f != OpenIddictConstants.Permissions.GrantTypes.Password &&
                f != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                f != OpenIddictConstants.Permissions.GrantTypes.DeviceCode).Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));

        CreateMap<ApplicationDto, ApplicationModalView>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));


        CreateMap<ApplicationTokenLifetimeDto, ApplicationTokenLifetimeModalView>().ReverseMap();
    }
}
