using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Contact;

public interface IContactPublicAppService : IApplicationService
{
    Task SendMessageAsync(ContactCreateInput input);
}
