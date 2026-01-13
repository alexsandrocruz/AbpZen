using AutoMapper;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;

namespace Sapienza.Lexus.advRevisaoDocumentos;

public class advRevisaoDocumentosAutoMapperProfile : Profile
{
    public advRevisaoDocumentosAutoMapperProfile()
    {
        CreateMap<advRevisaoDocumentos, advRevisaoDocumentosDto>();
        CreateMap<CreateUpdateadvRevisaoDocumentosDto, advRevisaoDocumentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvRevisaoDocumentosDto, advRevisaoDocumentos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
