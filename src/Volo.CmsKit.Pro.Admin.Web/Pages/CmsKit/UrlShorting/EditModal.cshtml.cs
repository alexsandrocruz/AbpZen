using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.UrlShorting;

public class EditModalModel : AdminPageModel
{
    protected IUrlShortingAdminAppService UrlShortingAdminAppService { get; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public ShortenedUrlEditViewModel ViewModel { get; set; }

    public EditModalModel(IUrlShortingAdminAppService urlShortingAdminAppService)
    {
        this.UrlShortingAdminAppService = urlShortingAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var dto = await UrlShortingAdminAppService.GetAsync(Id);

        ViewModel = ObjectMapper.Map<ShortenedUrlDto, ShortenedUrlEditViewModel>(dto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var tagDto = ObjectMapper.Map<ShortenedUrlEditViewModel, UpdateShortenedUrlDto>(ViewModel);
        await UrlShortingAdminAppService.UpdateAsync(Id, tagDto);
        return NoContent();
    }
    public class ShortenedUrlEditViewModel
    {
        [Required]
        [DisabledInput]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxSourceLength))]
        public string Source { get; set; }

        [Required]
        [DynamicMaxLength(typeof(ShortenedUrlConst), nameof(ShortenedUrlConst.MaxTargetLength))]
        [InputInfoText("UrlForwarding:EnsureTheUrlIsStartingWithSlashIfSameDomain")]
        public string Target { get; set; }
        
        [Display(Name = "UrlForwarding:IsRegex")]
        [DisabledInput]
        public bool IsRegex { get; set; }
    }
}
