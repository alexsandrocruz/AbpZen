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
        public IMongoCollection<Sapienza.Lexus.Client.Client> Clients => Collection<Sapienza.Lexus.Client.Client>();
        public IMongoCollection<Sapienza.Lexus.Specialization.Specialization> Specializations => Collection<Sapienza.Lexus.Specialization.Specialization>();
        public IMongoCollection<Sapienza.Lexus.LegalProcess.LegalProcess> LegalProcesses => Collection<Sapienza.Lexus.LegalProcess.LegalProcess>();
        public IMongoCollection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization> LawyerSpecializations => Collection<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>();
        public IMongoCollection<Sapienza.Lexus.Proposal.Proposal> Proposals => Collection<Sapienza.Lexus.Proposal.Proposal>();
                public IMongoCollection<Sapienza.Lexus.PropostalItem.PropostalItem> PropostalItems => Collection<Sapienza.Lexus.PropostalItem.PropostalItem>();
      // <GEN-MONGODB-COLLECTIONS>

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
