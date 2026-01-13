using AutoMapper;
using Sapienza.Lexus.fdtDevs.Dtos;

namespace Sapienza.Lexus.fdtDevs;

public class fdtDevsAutoMapperProfile : Profile
{
    public fdtDevsAutoMapperProfile()
    {
        CreateMap<fdtDevs, fdtDevsDto>();
        CreateMap<CreateUpdatefdtDevsDto, fdtDevs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefdtDevsDto, fdtDevs>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
