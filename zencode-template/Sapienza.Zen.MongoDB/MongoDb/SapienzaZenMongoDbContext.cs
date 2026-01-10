using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;

namespace Sapienza.Zen.MongoDB
{
    [ConnectionStringName("Default")]
    public class SapienzaZenMongoDbContext : AbpMongoDbContext
    {

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        // <<GEN-MONGODB-COLLECTIONS>>

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
