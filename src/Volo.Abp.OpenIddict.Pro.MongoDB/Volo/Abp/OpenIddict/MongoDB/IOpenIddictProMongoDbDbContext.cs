using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.OpenIddict.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpOpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictProMongoDbDbContext : IOpenIddictMongoDbContext
{

}
