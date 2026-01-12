using AutoMapper;
using LeptonXDemoApp.Edital.Dtos;

namespace LeptonXDemoApp.Edital;

public class EditalAutoMapperProfile : Profile
{
    public EditalAutoMapperProfile()
    {
        CreateMap<Edital, EditalDto>();
        CreateMap<CreateUpdateEditalDto, Edital>();
        CreateMap<CreateUpdateEditalDto, Edital>();
    }
}
