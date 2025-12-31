using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity;

public interface IIdentityExternalLoginAppService : IApplicationService
{
    Task CreateOrUpdateAsync();
}