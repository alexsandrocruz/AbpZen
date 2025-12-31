using AutoMapper;
using Volo.Abp.Account.Pro.Public.Blazor.Pages.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Pro.Public.Blazor;

public class AbpAccountBlazorAutoMapperProfile : Profile
{
    public AbpAccountBlazorAutoMapperProfile()
    {
        CreateMap<ProfileDto, PersonalInfoModel>()
            .MapExtraProperties();

        CreateMap<PersonalInfoModel, UpdateProfileDto>()
            .MapExtraProperties();
    }
}
