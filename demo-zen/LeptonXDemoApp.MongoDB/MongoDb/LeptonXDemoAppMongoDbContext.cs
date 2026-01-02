using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;
using EditalEntity = LeptonXDemoApp.Edital.Edital;
using ProductEntity = LeptonXDemoApp.Product.Product;
using CategoryEntity = LeptonXDemoApp.Category.Category;

namespace LeptonXDemoApp.MongoDB
{
    [ConnectionStringName("Default")]
    public class LeptonXDemoAppMongoDbContext : AbpMongoDbContext
    {

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        public IMongoCollection<EditalEntity> Editais => Collection<EditalEntity>();
        public IMongoCollection<ProductEntity> Products => Collection<ProductEntity>();
        public IMongoCollection<CategoryEntity> Categories => Collection<CategoryEntity>();
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
