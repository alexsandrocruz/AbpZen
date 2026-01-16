using AutoMapper;
using Sapienza.Lexus.advFornecedores.Dtos;

namespace Sapienza.Lexus.advFornecedores;

public class advFornecedoresAutoMapperProfile : Profile
{
    public advFornecedoresAutoMapperProfile()
    {
        CreateMap<advFornecedores, advFornecedoresDto>()
            .ForMember(dest => dest.advClientesDisplayName, opt => opt.MapFrom(src => src.advFornecedoresNav.apelido));
        CreateMap<CreateUpdateadvFornecedoresDto, advFornecedores>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvFornecedoresDto, advFornecedores>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
