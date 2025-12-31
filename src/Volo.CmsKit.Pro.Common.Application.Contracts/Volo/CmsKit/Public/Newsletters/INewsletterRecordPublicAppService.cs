using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Newsletters;

namespace Volo.CmsKit.Public.Newsletters;

public interface INewsletterRecordPublicAppService : IApplicationService
{
    Task CreateAsync(CreateNewsletterRecordInput input);

    Task<List<NewsletterPreferenceDetailsDto>> GetNewsletterPreferencesAsync(string emailAddress);

    Task UpdatePreferencesAsync(UpdatePreferenceRequestInput input);

    Task<NewsletterEmailOptionsDto> GetOptionByPreferenceAsync(string preference);
}
