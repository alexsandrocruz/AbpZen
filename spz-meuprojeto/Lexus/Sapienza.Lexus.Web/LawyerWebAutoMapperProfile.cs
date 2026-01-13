using AutoMapper;
using Sapienza.Lexus.Lawyer.Dtos;
using Sapienza.Lexus.Web.Pages.Lawyer.ViewModels;

namespace Sapienza.Lexus.Web;

public class LawyerWebAutoMapperProfile : Profile
{
    public LawyerWebAutoMapperProfile()
    {
        CreateMap<LawyerDto, EditLawyerViewModel>();
        CreateMap<CreateLawyerViewModel, CreateUpdateLawyerDto>();
        CreateMap<EditLawyerViewModel, CreateUpdateLawyerDto>();
    }
}
