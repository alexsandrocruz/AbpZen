using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Owl.reCAPTCHA;
using Owl.reCAPTCHA.v3;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contact;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Contact;

[RequiresFeature(CmsKitProFeatures.ContactEnable)]
[RequiresGlobalFeature(typeof(ContactFeature))]
[RemoteService(Name = CmsKitProCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/contacts")]
public class ContactPublicController : CmsKitProCommonController, IContactPublicAppService
{
    protected IContactPublicAppService ContactPublicAppService { get; }

    protected IreCAPTCHASiteVerifyV3 SiteVerify { get; }

    protected CmsKitContactOptions CmsKitContactOptions { get; }

    public ContactPublicController(
        IContactPublicAppService contactPublicAppService,
        IreCAPTCHASiteVerifyV3 siteVerify,
        IOptions<CmsKitContactOptions> cmsKitContactOptions
    )
    {
        ContactPublicAppService = contactPublicAppService;
        SiteVerify = siteVerify;
        CmsKitContactOptions = cmsKitContactOptions.Value;
    }

    [HttpPost]
    public virtual async Task SendMessageAsync(ContactCreateInput input)
    {
        if (await IsAllowedToSendMessageAsync(input.RecaptchaToken))
        {
            await ContactPublicAppService.SendMessageAsync(input);
        }
        else
        {
            throw new UserFriendlyException(L["RecaptchaError"]);
        }
    }

    private async Task<bool> IsAllowedToSendMessageAsync(string recaptchaToken)
    {
        if (!CmsKitContactOptions.IsRecaptchaEnabled)
        {
            return true;
        }

        var response = await SiteVerify.Verify(new reCAPTCHASiteVerifyRequest
        {
            Response = recaptchaToken,
            RemoteIp = HttpContext?.Connection?.RemoteIpAddress?.ToString()
        });

        return response.Success && response.Score > 0.5;
    }
}