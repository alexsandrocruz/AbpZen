using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpOpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictProDbContext : IOpenIddictDbContext
{

}
