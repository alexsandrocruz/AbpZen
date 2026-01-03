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
        public IMongoCollection<LeptonXDemoApp.Order.Order> Orders => Collection<LeptonXDemoApp.Order.Order>();
        public IMongoCollection<LeptonXDemoApp.Customer.Customer> Customers => Collection<LeptonXDemoApp.Customer.Customer>();
        public IMongoCollection<LeptonXDemoApp.OrderItem.OrderItem> OrderItems => Collection<LeptonXDemoApp.OrderItem.OrderItem>();
      public IMongoCollection<Lead> Leads => Collection<Lead>();
      public IMongoCollection<LeadContact> LeadContacts => Collection<LeadContact>();
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
