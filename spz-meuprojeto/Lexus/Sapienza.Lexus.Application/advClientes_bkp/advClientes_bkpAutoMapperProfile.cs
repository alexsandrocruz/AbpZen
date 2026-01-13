using AutoMapper;
using Sapienza.Lexus.advClientes_bkp.Dtos;

namespace Sapienza.Lexus.advClientes_bkp;

public class advClientes_bkpAutoMapperProfile : Profile
{
    public advClientes_bkpAutoMapperProfile()
    {
        CreateMap<advClientes_bkp, advClientes_bkpDto>();
        CreateMap<CreateUpdateadvClientes_bkpDto, advClientes_bkp>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientes_bkpDto, advClientes_bkp>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
