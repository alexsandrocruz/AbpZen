using AutoMapper;
using Sapienza.Lexus.fabLembretes.Dtos;

namespace Sapienza.Lexus.fabLembretes;

public class fabLembretesAutoMapperProfile : Profile
{
    public fabLembretesAutoMapperProfile()
    {
        CreateMap<fabLembretes, fabLembretesDto>();
        CreateMap<CreateUpdatefabLembretesDto, fabLembretes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefabLembretesDto, fabLembretes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
