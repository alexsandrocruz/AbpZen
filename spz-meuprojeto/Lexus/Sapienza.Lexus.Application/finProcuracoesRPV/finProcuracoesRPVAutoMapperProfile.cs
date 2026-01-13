using AutoMapper;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;

namespace Sapienza.Lexus.finProcuracoesRPV;

public class finProcuracoesRPVAutoMapperProfile : Profile
{
    public finProcuracoesRPVAutoMapperProfile()
    {
        CreateMap<finProcuracoesRPV, finProcuracoesRPVDto>();
        CreateMap<CreateUpdatefinProcuracoesRPVDto, finProcuracoesRPV>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdatefinProcuracoesRPVDto, finProcuracoesRPV>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
