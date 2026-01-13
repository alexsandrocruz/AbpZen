using AutoMapper;
using Sapienza.Lexus.Specialization.Dtos;
using Sapienza.Lexus.Web.Pages.Specialization.ViewModels;

namespace Sapienza.Lexus.Web;

public class SpecializationWebAutoMapperProfile : Profile
{
    public SpecializationWebAutoMapperProfile()
    {
        CreateMap<SpecializationDto, EditSpecializationViewModel>();
        CreateMap<CreateSpecializationViewModel, CreateUpdateSpecializationDto>();
        CreateMap<EditSpecializationViewModel, CreateUpdateSpecializationDto>();
    }
}
