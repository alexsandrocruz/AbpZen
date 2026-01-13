using AutoMapper;
using Sapienza.Lexus.advPreCheckLists.Dtos;

namespace Sapienza.Lexus.advPreCheckLists;

public class advPreCheckListsAutoMapperProfile : Profile
{
    public advPreCheckListsAutoMapperProfile()
    {
        CreateMap<advPreCheckLists, advPreCheckListsDto>();
        CreateMap<CreateUpdateadvPreCheckListsDto, advPreCheckLists>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreCheckListsDto, advPreCheckLists>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
