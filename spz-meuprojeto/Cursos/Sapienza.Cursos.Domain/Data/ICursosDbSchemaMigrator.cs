using System.Threading.Tasks;

namespace Sapienza.Cursos.Data
{
    public interface ICursosDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}