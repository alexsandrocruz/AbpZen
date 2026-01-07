using System.Threading.Tasks;

namespace Sapienza.Zen.Data
{
    public interface ISapienzaZenDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}