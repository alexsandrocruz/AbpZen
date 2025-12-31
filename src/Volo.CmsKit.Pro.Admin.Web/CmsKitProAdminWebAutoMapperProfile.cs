using AutoMapper;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Faqs;
using Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.PageFeedbacks;
using static Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls.EditModalModel;
using static Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls.CreateModalModel;
using EditModalModel = Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.UrlShorting.EditModalModel;
using CreateModalModel = Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.UrlShorting.CreateModalModel;

namespace Volo.CmsKit.Pro.Admin.Web;

public class CmsKitProAdminWebAutoMapperProfile : Profile
{
    public CmsKitProAdminWebAutoMapperProfile()
    {
        CreateMap<ShortenedUrlDto, EditModalModel.ShortenedUrlEditViewModel>();
        CreateMap<EditModalModel.ShortenedUrlEditViewModel, UpdateShortenedUrlDto>();
        CreateMap<CreateModalModel.CreateShortenedUrlViewModel, CreateShortenedUrlDto>();

        CreateMap<CreatePollViewModel, CreatePollDto>().MapExtraProperties();
        CreateMap<PollWithDetailsDto, PollEditViewModel>().MapExtraProperties();
        CreateMap<PollEditViewModel, UpdatePollDto>().MapExtraProperties();

        CreateMap<PageFeedbackDto, Pages.CmsKit.PageFeedbacks.EditModalModel.PageFeedbackEditViewModel>();
        CreateMap<Pages.CmsKit.PageFeedbacks.EditModalModel.PageFeedbackEditViewModel, UpdatePageFeedbackDto>();
        CreateMap<PageFeedbackDto, ViewModal.PageFeedbackViewModel>();
        CreateMap<PageFeedbackSettingDto, SettingsModal.PageFeedbackSettingViewModel>();
        CreateMap<SettingsModal.PageFeedbackSettingViewModel, UpdatePageFeedbackSettingDto>();

        CreateMap<FaqQuestionDto, EditQuestionModalModel.FaqQuestionEditViewModel>();
        CreateMap<EditQuestionModalModel.FaqQuestionEditViewModel, UpdateFaqQuestionDto>();

        CreateMap<CreateSectionModalModel.CreateFaqSectionViewModel, CreateFaqSectionDto>();
        CreateMap<CreateQuestionModalModel.FaqQuestionViewModel, CreateFaqQuestionDto>();
    }
}
