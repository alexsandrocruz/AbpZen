using System.Threading.Tasks;

namespace LeptonXDemoApp.Data
{
    public interface ILeptonXDemoAppDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}