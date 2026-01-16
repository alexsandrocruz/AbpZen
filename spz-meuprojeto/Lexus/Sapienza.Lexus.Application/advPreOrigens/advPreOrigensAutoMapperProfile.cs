using AutoMapper;
using Sapienza.Lexus.advPreOrigens.Dtos;

namespace Sapienza.Lexus.advPreOrigens;

public class advPreOrigensAutoMapperProfile : Profile
{
    public advPreOrigensAutoMapperProfile()
    {
        CreateMap<advPreOrigens, advPreOrigensDto>();
        CreateMap<CreateUpdateadvPreOrigensDto, advPreOrigens>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreOrigensDto, advPreOrigens>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
