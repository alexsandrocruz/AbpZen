using Volo.FileManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.FileManagement;

public abstract class FileManagementController : AbpControllerBase
{
    protected FileManagementController()
    {
        LocalizationResource = typeof(FileManagementResource);
    }
}
