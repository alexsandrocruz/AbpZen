using System;
using AutoMapper;
using System.Linq;
using Volo.Abp.AutoMapper;

namespace Volo.Abp.Identity;

public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
{
    public AbpIdentityApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties()
            .Ignore(x => x.IsLockedOut)
            .Ignore(x => x.SupportTwoFactor)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityRole, IdentityRoleDto>()
            .Ignore(x => x.UserCount)
            .MapExtraProperties();

        CreateMap<IdentityClaimType, ClaimTypeDto>()
            .MapExtraProperties()
            .Ignore(x => x.ValueTypeAsString);

        CreateMap<IdentityUserClaim, IdentityUserClaimDto>();

        CreateMap<IdentityUserClaimDto, IdentityUserClaim>()
            .Ignore(x => x.TenantId)
            .Ignore(x => x.Id);

        CreateMap<IdentityRoleClaim, IdentityRoleClaimDto>();

        CreateMap<IdentityRoleClaimDto, IdentityRoleClaim>()
            .Ignore(x => x.TenantId)
            .Ignore(x => x.Id);

        CreateMap<CreateClaimTypeDto, IdentityClaimType>()
            .MapExtraProperties()
            .Ignore(x => x.IsStatic)
            .Ignore(x => x.Id);

        CreateMap<UpdateClaimTypeDto, IdentityClaimType>()
            .MapExtraProperties()
            .Ignore(x => x.IsStatic)
            .Ignore(x => x.Id);

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
            .MapExtraProperties();

        CreateMap<OrganizationUnitRole, OrganizationUnitRoleDto>();

        CreateMap<OrganizationUnit, OrganizationUnitWithDetailsDto>()
            .MapExtraProperties()
            .Ignore(x => x.Roles)
            .Ignore(x => x.UserCount);

        CreateMap<IdentityRole, OrganizationUnitRoleDto>()
            .ForMember(dest => dest.RoleId, src => src.MapFrom(r => r.Id));

        CreateMap<IdentitySecurityLog, IdentitySecurityLogDto>();

        CreateMap<IdentityRole, IdentityRoleLookupDto>();

        CreateMap<OrganizationUnit, OrganizationUnitLookupDto>();

        CreateMap<IdentityUser, IdentityUserExportDto>()
            .ForMember(dest => dest.Active, src => src.MapFrom(r => r.IsActive ? "Yes" : "No"))
            .ForMember(dest => dest.EmailConfirmed, src => src.MapFrom(r => r.EmailConfirmed ? "Yes" : "No"))
            .ForMember(dest => dest.TwoFactorEnabled, src => src.MapFrom(r => r.TwoFactorEnabled ? "Yes" : "No"))
            .ForMember(dest => dest.AccountLookout, src => src.MapFrom(r => r.LockoutEnd != null && r.LockoutEnd > DateTime.UtcNow ? "Yes" : "No"))
            .Ignore(x => x.Roles);

        CreateMap<IdentitySession, IdentitySessionDto>()
            .ForMember(x => x.IpAddresses, s => s.MapFrom(x => x.GetIpAddresses()))
            .Ignore(x => x.TenantName)
            .Ignore(x => x.UserName)
            .Ignore(x => x.IsCurrent)
            .MapExtraProperties();
    }
}
