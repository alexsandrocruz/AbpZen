using System;
using AutoMapper;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Volo.Abp.Account;

public class AbpAccountPubicApplicationModuleAutoMapperProfile : Profile
{
    public AbpAccountPubicApplicationModuleAutoMapperProfile()
    {
        CreateMap<ExternalProviderSettings, ExternalProviderItemDto>(MemberList.Destination);
        CreateMap<ExternalProviderSettings, ExternalProviderItemWithSecretDto>(MemberList.Destination)
            .ForMember(d => d.Success, opt => opt.MapFrom(x => !x.Name.IsNullOrWhiteSpace()));

        CreateMap<IdentityUser, ProfileDto>()
            .ForMember(dest => dest.HasPassword,
                op => op.MapFrom(src => src.PasswordHash != null))
            .Ignore(x => x.SupportsMultipleTimezone)
            .Ignore(x => x.Timezone)
            .MapExtraProperties();

        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties()
            .Ignore(x => x.IsLockedOut)
            .Ignore(x => x.SupportTwoFactor)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentitySecurityLog, IdentitySecurityLogDto>();

        CreateMap<IdentityUser, UserLookupDto>()
            .ForMember(dest => dest.UserName, src => src.MapFrom(x => $"{x.UserName} ({x.Email})"));

        CreateMap<IdentitySession, IdentitySessionDto>()
            .ForMember(x => x.IpAddresses, s => s.MapFrom(x => x.GetIpAddresses()))
            .Ignore(x => x.TenantName)
            .Ignore(x => x.UserName)
            .Ignore(x => x.IsCurrent)
            .MapExtraProperties();
    }
}
