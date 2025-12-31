using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Faqs;
using Volo.CmsKit.Public.Faqs;

namespace Volo.CmsKit.Pro.Public.Web;

public class CmsKitProPublicWebAutoMapperProfile : Profile
{
    public CmsKitProPublicWebAutoMapperProfile()
    {
        CreateMap<FaqQuestionDto, FaqQuestionViewModel>();

        CreateMap<FaqSectionDto, FaqSectionModel>();
    }
}
