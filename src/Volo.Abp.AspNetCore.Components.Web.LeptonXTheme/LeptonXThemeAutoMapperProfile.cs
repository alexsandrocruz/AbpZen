using AutoMapper;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;
using Volo.Abp.AutoMapper;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme
{
    public class LeptonXThemeAutoMapperProfile : Profile
    {
        public LeptonXThemeAutoMapperProfile()
        {
            CreateMap<Volo.Abp.UI.Navigation.ApplicationMenu, MenuViewModel>()
                .ForMember(vm => vm.Menu, cnf => cnf.MapFrom(x => x));

            CreateMap<ApplicationMenuItem, MenuItemViewModel>()
                .ForMember(vm => vm.MenuItem, cnf => cnf.MapFrom(x => x))
                .Ignore(vm => vm.IsActive)
                .Ignore(vm => vm.IsOpen)
                .Ignore(vm => vm.Parent);
        }
    }
}
