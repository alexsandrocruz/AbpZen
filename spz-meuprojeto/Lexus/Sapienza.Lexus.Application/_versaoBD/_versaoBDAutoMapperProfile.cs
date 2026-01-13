using AutoMapper;
using Sapienza.Lexus._versaoBD.Dtos;

namespace Sapienza.Lexus._versaoBD;

public class _versaoBDAutoMapperProfile : Profile
{
    public _versaoBDAutoMapperProfile()
    {
        CreateMap<_versaoBD, _versaoBDDto>();
        CreateMap<CreateUpdate_versaoBDDto, _versaoBD>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdate_versaoBDDto, _versaoBD>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
