using System.Threading.Tasks;

namespace Sapienza.Dominus.Data
{
    public interface IDominusDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}