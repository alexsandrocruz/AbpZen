using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contact;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Contact;

[RequiresFeature(CmsKitProFeatures.ContactEnable)]
[RequiresGlobalFeature(ContactFeature.Name)]
public class ContactPublicAppService : ApplicationService, IContactPublicAppService
{
    protected ContactEmailSender ContactEmailSender { get; }
    private readonly CmsKitContactConfigOptions _cmsKitContactOptions;

    public ContactPublicAppService(
        ContactEmailSender contactEmailSender,
        IOptions<CmsKitContactConfigOptions> cmsKitContactOptions)
    {
        ContactEmailSender = contactEmailSender;
        _cmsKitContactOptions = cmsKitContactOptions.Value;
    }

    public virtual async Task SendMessageAsync(ContactCreateInput input)
    {
        await ContactEmailSender.SendAsync(input.ContactName, input.Name, input.Subject, input.Email, input.Message);
    }
}
