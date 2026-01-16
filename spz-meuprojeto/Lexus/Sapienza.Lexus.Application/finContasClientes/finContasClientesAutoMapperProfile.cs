using AutoMapper;
using Sapienza.Lexus.finContasClientes.Dtos;

namespace Sapienza.Lexus.finContasClientes;

public class finContasClientesAutoMapperProfile : Profile
{
    public finContasClientesAutoMapperProfile()
    {
        CreateMap<finContasClientes, finContasClientesDto>();
        CreateMap<CreateUpdatefinContasClientesDto, finContasClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinContasClientesDto, finContasClientes>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
