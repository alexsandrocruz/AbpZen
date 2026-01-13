using AutoMapper;
using Sapienza.Lexus.advPreProcessosCheckLists.Dtos;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

public class advPreProcessosCheckListsAutoMapperProfile : Profile
{
    public advPreProcessosCheckListsAutoMapperProfile()
    {
        CreateMap<advPreProcessosCheckLists, advPreProcessosCheckListsDto>();
        CreateMap<CreateUpdateadvPreProcessosCheckListsDto, advPreProcessosCheckLists>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreProcessosCheckListsDto, advPreProcessosCheckLists>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
