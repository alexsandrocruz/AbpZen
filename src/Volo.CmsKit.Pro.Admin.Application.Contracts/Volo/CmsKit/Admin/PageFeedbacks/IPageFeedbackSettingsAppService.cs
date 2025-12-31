using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.PageFeedbacks;

public interface IPageFeedbackSettingsAppService : IApplicationService
{
    Task<CmsKitPageFeedbackSettingDto> GetAsync();

    Task UpdateAsync(UpdateCmsKitPageFeedbackSettingDto input);
}