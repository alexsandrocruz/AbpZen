using System.Threading.Tasks;

namespace Sapienza.Lexus.Data
{
    public interface ILexusDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}