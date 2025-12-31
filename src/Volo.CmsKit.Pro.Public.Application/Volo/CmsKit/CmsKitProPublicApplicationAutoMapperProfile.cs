using System.Collections.Generic;
using AutoMapper;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.Public.Faqs;
using Volo.CmsKit.Public.Newsletters;
using Volo.CmsKit.Public.PageFeedbacks;
using Volo.CmsKit.Public.Polls;
using Volo.CmsKit.Public.UrlShorting;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit;

public class CmsKitProPublicApplicationAutoMapperProfile : Profile
{
    public CmsKitProPublicApplicationAutoMapperProfile()
    {
        CreateMap<NewsletterPreferenceDefinition, NewsletterPreferenceDetailsDto>()
            .Ignore(x => x.DisplayPreference)
            .Ignore(x => x.Definition)
            .Ignore(x => x.IsSelectedByEmailAddress);

        CreateMap<NewsletterPreferenceDefinition, NewsletterEmailOptionsDto>()
            .Ignore(x => x.PrivacyPolicyConfirmation)
            .Ignore(x => x.DisplayAdditionalPreferences)
            .Ignore(x => x.AdditionalPreferences);

        CreateMap<ShortenedUrl, ShortenedUrlDto>();
        CreateMap<ShortenedUrl, ShortenedUrlCacheItem>().Ignore(x => x.Exists);
        CreateMap<ShortenedUrlCacheItem, ShortenedUrlDto>();
        CreateMap<ShortenedUrlDto, ShortenedUrlCacheItem>().Ignore(x => x.Exists);

        CreateMap<Poll, PollWithDetailsDto>();
        CreateMap<PollOption, PollOptionDto>();
        
        CreateMap<PageFeedback, PageFeedbackDto>();

        CreateMap<FaqSectionWithQuestions, FaqSectionWithQuestionsDto>();

        CreateMap<FaqQuestion, FaqQuestionDto>();

        CreateMap<FaqSection, FaqSectionDto>();
    }
}
