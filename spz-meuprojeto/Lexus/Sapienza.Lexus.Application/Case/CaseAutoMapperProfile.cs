using AutoMapper;
using Sapienza.Lexus.Case.Dtos;

namespace Sapienza.Lexus.Case;

public class CaseAutoMapperProfile : Profile
{
    public CaseAutoMapperProfile()
    {
        CreateMap<Case, CaseDto>();
        CreateMap<CreateUpdateCaseDto, Case>();
        CreateMap<CreateUpdateCaseDto, Case>();
    }
}
