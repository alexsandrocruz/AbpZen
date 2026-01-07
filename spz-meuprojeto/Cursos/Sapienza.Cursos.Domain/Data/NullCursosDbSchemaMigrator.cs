using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Cursos.Data
{
    /* This is used if database provider does't define
     * ICursosDbSchemaMigrator implementation.
     */
    public class NullCursosDbSchemaMigrator : ICursosDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}