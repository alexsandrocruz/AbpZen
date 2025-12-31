using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.Tags;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit;

public class CmsKitProAdminApplicationAutoMapperProfile : Profile
{
    public CmsKitProAdminApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<NewsletterRecord, NewsletterRecordWithDetailsDto>()
            .ForMember(s => s.Preferences, c => c.MapFrom(m => m.Preferences))
            .MapExtraProperties();

        CreateMap<NewsletterPreference, NewsletterPreferenceDto>();

        CreateMap<NewsletterSummaryQueryResultItem, NewsletterRecordDto>()
            .Ignore(x => x.ExtraProperties);

        CreateMap<TagDto, TagCreateDto>().MapExtraProperties();

        CreateMap<TagDto, TagUpdateDto>().MapExtraProperties();

        CreateMap<TagCreateDto, Tag>(MemberList.Source)
            .Ignore(x => x.Id)
            .MapExtraProperties();

        CreateMap<TagUpdateDto, Tag>(MemberList.Source)
            .MapExtraProperties();

        CreateMap<ShortenedUrl, ShortenedUrlDto>();

        CreateMap<Poll, PollDto>().MapExtraProperties();
        CreateMap<Poll, PollWithDetailsDto>().MapExtraProperties();

        CreateMap<PollOption, PollOptionDto>();

        CreateMap<PageFeedback, PageFeedbackDto>();

        CreateMap<PageFeedbackDto, UpdatePageFeedbackDto>();
        CreateMap<PageFeedbackSetting, PageFeedbackSettingDto>();

        CreateMap<FaqSection, FaqSectionDto>();
        CreateMap<FaqSectionWithQuestionCount, FaqSectionWithQuestionCountDto>();
        
        CreateMap<FaqQuestion, FaqQuestionDto>();
        
        CreateMap<FaqGroupInfo, FaqGroupInfoDto>();
    }
}

