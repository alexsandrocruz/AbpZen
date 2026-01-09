using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;
using Sapienza.Lexus.Lawyer;
using MongoDB.Driver;

namespace Sapienza.Lexus.MongoDB
{
    [ConnectionStringName("Default")]
    public class LexusMongoDbContext : AbpMongoDbContext
    {

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        public IMongoCollection<Sapienza.Lexus.Lawyer.Lawyer> Lawyers => Collection<Sapienza.Lexus.Lawyer.Lawyer>();

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
