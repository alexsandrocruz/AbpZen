using AutoMapper;
using Sapienza.Lexus.flwFollows.Dtos;

namespace Sapienza.Lexus.flwFollows;

public class flwFollowsAutoMapperProfile : Profile
{
    public flwFollowsAutoMapperProfile()
    {
        CreateMap<flwFollows, flwFollowsDto>();
        CreateMap<CreateUpdateflwFollowsDto, flwFollows>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateflwFollowsDto, flwFollows>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
