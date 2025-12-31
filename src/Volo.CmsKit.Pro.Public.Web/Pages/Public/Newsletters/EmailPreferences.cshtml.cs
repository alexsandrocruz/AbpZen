using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Localization;
using Volo.CmsKit.Newsletters.Helpers;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Pro.Public.Web.Pages.Public.Newsletters;

public class EmailPreferencesModel : AbpPageModel
{
    protected INewsletterRecordPublicAppService NewsletterRecordPublicAppService { get; }

    protected SecurityCodeProvider SecurityCodeProvider { get; }
    
    public List<NewsletterPreferenceDetailsViewModel> NewsletterPreferenceDetailsViewModels { get; set; }

    public string EmailAddress { get; set; }

    public ILocalizableString PrivacyPolicyConfirmationMessage { get; set; }
    
    public string Source { get; set; }
    
    public string SecurityCode { get; set; }

    public NewsletterPreferencesManagementOptions NewsletterPreferencesManagementOption { get; set; }

    public EmailPreferencesModel(
        INewsletterRecordPublicAppService newsletterRecordPublicAppService,
        SecurityCodeProvider securityCodeProvider,
        IOptions<NewsletterPreferencesManagementOptions> newsletterPreferencesManagementOptions)
    {
        NewsletterRecordPublicAppService = newsletterRecordPublicAppService;
        SecurityCodeProvider = securityCodeProvider;

        NewsletterPreferenceDetailsViewModels = new List<NewsletterPreferenceDetailsViewModel>();

        NewsletterPreferencesManagementOption = newsletterPreferencesManagementOptions.Value;
    }

    public async Task<IActionResult> OnGetAsync(string emailAddress, string securityCode)
    {
        SetSourceAndPrivacyMessage();

        if (securityCode.IsNullOrWhiteSpace() && emailAddress.IsNullOrWhiteSpace() && CurrentUser.IsAuthenticated && !CurrentUser.Email.IsNullOrWhiteSpace())
        {
            EmailAddress = CurrentUser.Email;
            SecurityCode = SecurityCodeProvider.GetSecurityCode(EmailAddress);
        }
        else if (!CurrentUser.IsAuthenticated && (securityCode.IsNullOrWhiteSpace() || emailAddress.IsNullOrWhiteSpace()))
        {
            return Redirect($"~/Account/Login?ReturnUrl={HttpContext.Request.Path.Value}");
        }
        else if(emailAddress.IsNullOrWhiteSpace() || securityCode.IsNullOrWhiteSpace())
        {
            return Unauthorized();
        }
        else
        {
            var hashSecurityCode = SecurityCodeProvider.GetSecurityCode(emailAddress);

            if (securityCode != hashSecurityCode)
            {
                return Unauthorized();
            }

            EmailAddress = emailAddress;
            SecurityCode = securityCode;
        }
        
        var newsletterSubscribeDetailsDto = await NewsletterRecordPublicAppService.GetNewsletterPreferencesAsync(EmailAddress);

        foreach (var newsletterPreference in newsletterSubscribeDetailsDto)
        {
            NewsletterPreferenceDetailsViewModels.Add(new NewsletterPreferenceDetailsViewModel
            {
                Preference = newsletterPreference.Preference,
                DisplayPreference = newsletterPreference.DisplayPreference,
                DisplayDefinition = newsletterPreference.Definition,
                IsSelectedByEmailAddress = newsletterPreference.IsSelectedByEmailAddress
            });
        }

        return Page();
    }
    
    public void SetSourceAndPrivacyMessage()
    {
        PrivacyPolicyConfirmationMessage = NewsletterPreferencesManagementOption.PrivacyPolicyConfirmation;
        Source = NewsletterPreferencesManagementOption.Source;
    }
}

public class NewsletterPreferenceDetailsViewModel
{
    public string Preference { get; set; }

    public string DisplayPreference { get; set; }

    public string DisplayDefinition { get; set; }

    public bool IsSelectedByEmailAddress { get; set; }
}
