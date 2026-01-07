using AutoMapper;
using Sapienza.Cursos.NewEntity1.Dtos;

namespace Sapienza.Cursos.NewEntity1;

public class NewEntity1AutoMapperProfile : Profile
{
    public NewEntity1AutoMapperProfile()
    {
        CreateMap<NewEntity1, NewEntity1Dto>();
        CreateMap<CreateUpdateNewEntity1Dto, NewEntity1>();
        CreateMap<CreateUpdateNewEntity1Dto, NewEntity1>();
    }
}
