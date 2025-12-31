using AutoMapper;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;
using Volo.Abp.AutoMapper;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.ObjectMapping
{
    public class LeptonXThemeAutoMapperProfile : Profile
    {
        public LeptonXThemeAutoMapperProfile()
        {
            CreateMap<ApplicationMenu, MenuViewModel>()
                .ForMember(vm => vm.Menu, cnf => cnf.MapFrom(x => x));

            CreateMap<ApplicationMenuItem, MenuItemViewModel>()
                .ForMember(vm => vm.MenuItem, cnf => cnf.MapFrom(x => x))
                .Ignore(vm => vm.IsActive)
                .Ignore(vm => vm.IsInRoot);
        }
    }
}
