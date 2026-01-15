using AutoMapper;
using Sapienza.Dominus.LeadTag.Dtos;

namespace Sapienza.Dominus.LeadTag;

public class LeadTagAutoMapperProfile : Profile
{
    public LeadTagAutoMapperProfile()
    {
        CreateMap<LeadTag, LeadTagDto>();
        CreateMap<CreateUpdateLeadTagDto, LeadTag>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLeadTagDto, LeadTag>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
