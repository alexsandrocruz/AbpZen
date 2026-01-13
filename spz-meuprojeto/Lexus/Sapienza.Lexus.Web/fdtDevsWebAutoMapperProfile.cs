using AutoMapper;
using Sapienza.Lexus.fdtDevs.Dtos;
using Sapienza.Lexus.Web.Pages.fdtDevs.ViewModels;

namespace Sapienza.Lexus.Web;

public class fdtDevsWebAutoMapperProfile : Profile
{
    public fdtDevsWebAutoMapperProfile()
    {
        CreateMap<fdtDevsDto, EditfdtDevsViewModel>();
        CreateMap<CreatefdtDevsViewModel, CreateUpdatefdtDevsDto>();
        CreateMap<EditfdtDevsViewModel, CreateUpdatefdtDevsDto>();
    }
}
