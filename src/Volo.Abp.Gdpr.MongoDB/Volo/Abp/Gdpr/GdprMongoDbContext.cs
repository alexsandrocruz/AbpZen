using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Gdpr;

[ConnectionStringName(GdprDbProperties.ConnectionStringName)]
public class GdprMongoDbContext : AbpMongoDbContext, IGdprMongoDbContext
{
    public IMongoCollection<GdprRequest> GdprRequests => Collection<GdprRequest>();
    
    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureGdpr();
    }
}