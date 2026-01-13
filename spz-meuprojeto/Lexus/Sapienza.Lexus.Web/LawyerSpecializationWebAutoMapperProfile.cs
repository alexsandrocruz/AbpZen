using AutoMapper;
using Sapienza.Lexus.LawyerSpecialization.Dtos;
using Sapienza.Lexus.Web.Pages.LawyerSpecialization.ViewModels;

namespace Sapienza.Lexus.Web;

public class LawyerSpecializationWebAutoMapperProfile : Profile
{
    public LawyerSpecializationWebAutoMapperProfile()
    {
        CreateMap<LawyerSpecializationDto, EditLawyerSpecializationViewModel>();
        CreateMap<CreateLawyerSpecializationViewModel, CreateUpdateLawyerSpecializationDto>();
        CreateMap<EditLawyerSpecializationViewModel, CreateUpdateLawyerSpecializationDto>();
    }
}
