using AutoMapper;
using Sapienza.Lexus.RAGDoc.Dtos;

namespace Sapienza.Lexus.RAGDoc;

public class RAGDocAutoMapperProfile : Profile
{
    public RAGDocAutoMapperProfile()
    {
        CreateMap<RAGDoc, RAGDocDto>();
        CreateMap<CreateUpdateRAGDocDto, RAGDoc>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateRAGDocDto, RAGDoc>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
