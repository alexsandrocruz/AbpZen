using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Gdpr;

[ConnectionStringName(GdprDbProperties.ConnectionStringName)]
public interface IGdprMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<GdprRequest> GdprRequests { get; }
}