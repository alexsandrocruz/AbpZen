using AutoMapper;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Applications.Dtos;
using Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Pro.Web.Pages.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Scopes.Dtos;

namespace Volo.Abp.OpenIddict.Pro.Web;

public class AbpOpenIddictProWebAutoMapperProfile : Profile
{
    public AbpOpenIddictProWebAutoMapperProfile()
    {
        CreateMap<ScopeCreateModalView, CreateScopeInput>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ScopeDto, ScopeEditModelView>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ScopeEditModelView, UpdateScopeInput>()
            .MapExtraProperties()
            .ForMember(x => x.Resources, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.Resources)));

        CreateMap<ApplicationCreateModalView, CreateApplicationInput>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => new HashSetStringConverter(f => f != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                f != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                f != OpenIddictConstants.Permissions.GrantTypes.Implicit &&
                f != OpenIddictConstants.Permissions.GrantTypes.Password &&
                f != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                f != OpenIddictConstants.Permissions.GrantTypes.DeviceCode).Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));

        CreateMap<ApplicationDto, ApplicationEditModalView>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));

        CreateMap<ApplicationEditModalView, UpdateApplicationInput>()
            .MapExtraProperties()
            .ForMember(x => x.ExtensionGrantTypes, opts => opts.MapFrom(x => new HashSetStringConverter(f => f != OpenIddictConstants.Permissions.GrantTypes.RefreshToken &&
                f != OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode &&
                f != OpenIddictConstants.Permissions.GrantTypes.Implicit &&
                f != OpenIddictConstants.Permissions.GrantTypes.Password &&
                f != OpenIddictConstants.Permissions.GrantTypes.ClientCredentials &&
                f != OpenIddictConstants.Permissions.GrantTypes.DeviceCode).Convert(x.ExtensionGrantTypes)))
            .ForMember(x => x.RedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.RedirectUris)))
            .ForMember(x => x.PostLogoutRedirectUris, opts => opts.MapFrom(x => HashSetStringConverter.Instance.Convert(x.PostLogoutRedirectUris)));

        CreateMap<ApplicationTokenLifetimeDto, ApplicationTokenLifetimeModalView>().ReverseMap();
    }
}
