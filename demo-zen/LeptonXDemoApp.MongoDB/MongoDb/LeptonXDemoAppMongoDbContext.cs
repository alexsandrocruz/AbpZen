using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;

namespace LeptonXDemoApp.MongoDB
{
    [ConnectionStringName("Default")]
    public class LeptonXDemoAppMongoDbContext : AbpMongoDbContext
    {

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        // <ZenCode-MongoCollections-Marker>

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureChat();
            modelBuilder.ConfigureFileManagement();
            //builder.Entity<YourEntity>(b =>
            //{
            //    //...
            //});
        }
    }
}
