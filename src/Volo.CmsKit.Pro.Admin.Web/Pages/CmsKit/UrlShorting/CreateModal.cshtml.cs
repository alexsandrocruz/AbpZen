using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.UrlShorting;

public class CreateModalModel : AdminPageModel
{
    protected IUrlShortingAdminAppService UrlShortingAdminAppService { get; }

    [BindProperty]
    public CreateShortenedUrlViewModel ViewModel { get; set; }

    public CreateModalModel(IUrlShortingAdminAppService urlShortingAdminAppService)
    {
        UrlShortingAdminAppService = urlShortingAdminAppService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<CreateShortenedUrlViewModel, CreateShortenedUrlDto>(ViewModel);

        await UrlShortingAdminAppService.CreateAsync(dto);

        return NoContent();
    }

    public class CreateShortenedUrlViewModel
    {
        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxSourceLength))]
        public string Source { get; set; }

        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxTargetLength))]
        [InputInfoText("UrlForwarding:EnsureTheUrlIsStartingWithSlashIfSameDomain")]
        public string Target { get; set; }
        
        [Display(Name = "UrlForwarding:IsRegex")]
        public bool IsRegex { get; set; }
    }
}
