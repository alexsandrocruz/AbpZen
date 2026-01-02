using MongoDB.Driver;
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
        public IMongoCollection<LeptonXDemoApp.Edital.Edital> Editais => Collection<LeptonXDemoApp.Edital.Edital>();
        public IMongoCollection<LeptonXDemoApp.Product.Product> Products => Collection<LeptonXDemoApp.Product.Product>();
        public IMongoCollection<LeptonXDemoApp.Category.Category> Categories => Collection<LeptonXDemoApp.Category.Category>();
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
