using AutoMapper;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Admin.Menus;
using Volo.CmsKit.Admin.PageFeedbacks;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;
using Volo.CmsKit.Tags;
using static Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.MenuManagement;

namespace Volo.CmsKit.Pro.Admin.Blazor;

public class CmsKitProAdminBlazorAutoMapperProfile : Profile
{
    public CmsKitProAdminBlazorAutoMapperProfile()
    {
        CreateMap<ShortenedUrlDto, UpdateShortenedUrlDto>();

        CreateMap<BlogPostDto, UpdateBlogPostDto>();

        CreateMap<BlogDto, UpdateBlogDto>();

        CreateMap<CommentWithAuthorDto, CommentManagement.CommentWithAuthorViewModel>()
            .ForMember(x => x.UserName,
                opt => opt.MapFrom(x => x.Author.UserName));

        CreateMap<MenuItemDto, MenuItemUpdateInput>();

        CreateMap<PollWithDetailsDto, UpdatePollDto>();

        CreateMap<TagDto, TagUpdateDto>();

        CreateMap<PageFeedbackSettingDto, UpdatePageFeedbackSettingDto>();

        CreateMap<MenuManagement.MenuItemUpdateViewModel, MenuItemUpdateInput>();
        CreateMap<MenuItemWithDetailsDto, MenuManagement.MenuItemUpdateViewModel>();
        CreateMap<MenuItemCreateViewModel, MenuItemCreateInput>();
    }
}