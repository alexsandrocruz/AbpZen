using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplating;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Templates;

namespace Volo.CmsKit.Contact;

public class ContactEmailSender : ITransientDependency
{
    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }
    protected IStringLocalizer<CmsKitResource> Localizer { get; }
    protected ISettingManager SettingManager { get; }

    private readonly CmsKitContactConfigOptions _cmsKitContactOptions;


    public ContactEmailSender(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer,
        IStringLocalizer<CmsKitResource> localizer,
        ISettingManager settingManager,
        IOptions<CmsKitContactConfigOptions> cmsKitContactOptions)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;
        Localizer = localizer;
        SettingManager = settingManager;
        _cmsKitContactOptions = cmsKitContactOptions.Value;
    }

    public virtual async Task SendAsync(string contactName, string name, string subject, string email, string message)
    {
        var contactConfig = _cmsKitContactOptions.ContactConfigs.FirstOrDefault(c => c.Key == contactName).Value;
        string receiverEmailAddress;
        if (contactConfig is null || string.IsNullOrEmpty(contactConfig.ReceiverEmailAddress))
        {
            receiverEmailAddress = await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.Contact.ReceiverEmailAddress);
        }
        else
        {
            receiverEmailAddress = contactConfig.ReceiverEmailAddress;
        }

        if (string.IsNullOrWhiteSpace(receiverEmailAddress))
        {
            throw new ArgumentNullException(Localizer["EmailToException"]);
        }

        var body = await TemplateRenderer.RenderAsync(
            CmsKitEmailTemplates.ContactEmailTemplate,
            new {
                Title = Localizer["Contact"],
                Name = $"{Localizer["Name"]} : {name}",
                Email = $"{Localizer["EmailAddress"]} : {email}",
                Message = $"{Localizer["Message"]} : {message}"
            }
        );

        if (!string.IsNullOrWhiteSpace(contactName))
        {
            subject = $"{contactName}: {subject}";
        }
        await EmailSender.SendAsync(receiverEmailAddress, subject, body);
    }
}
