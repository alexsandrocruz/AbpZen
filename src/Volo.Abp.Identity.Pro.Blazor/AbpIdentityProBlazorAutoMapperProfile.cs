using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

namespace Volo.Abp.Identity.Pro.Blazor;

public class AbpIdentityProBlazorAutoMapperProfile : Profile
{
    public AbpIdentityProBlazorAutoMapperProfile()
    {
        CreateMap<IdentityUserDto, IdentityUserUpdateDto>()
            .MapExtraProperties()
            .Ignore(x => x.OrganizationUnitIds)
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>()
            .MapExtraProperties();

        CreateMap<ClaimTypeDto, UpdateClaimTypeDto>()
            .MapExtraProperties();

        CreateMap<OrganizationUnitWithDetailsDto, OrganizationUnitTreeView>()
            .MapExtraProperties()
            .Ignore(x => x.Children)
            .Ignore(x => x.HasChildren)
            .Ignore(x => x.Collapsed);

        CreateMap<OrganizationUnitWithDetailsDto, OrganizationUnitUpdateDto>()
            .MapExtraProperties();
    }
}
