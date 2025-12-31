using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Contact;

public interface IContactSettingsAppService : IApplicationService
{
    Task<CmsKitContactSettingDto> GetAsync();

    Task UpdateAsync(UpdateCmsKitContactSettingDto input);
}
