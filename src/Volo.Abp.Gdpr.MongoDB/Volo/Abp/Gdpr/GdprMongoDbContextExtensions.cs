using Volo.Abp.MongoDB;

namespace Volo.Abp.Gdpr;

public static class GdprMongoDbContextExtensions
{
    public static void ConfigureGdpr(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.Entity<GdprRequest>(b =>
        {
            b.CollectionName = GdprDbProperties.DbTablePrefix + "Requests";
        });
        
        builder.Entity<GdprInfo>(b =>
        {
            b.CollectionName = GdprDbProperties.DbTablePrefix + "Info";
        });
    }
}