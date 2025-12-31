using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

[ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
public interface IIdentityProMongoDbContext : IAbpIdentityMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
