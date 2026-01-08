using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Lexus.Data
{
    /* This is used if database provider does't define
     * ILexusDbSchemaMigrator implementation.
     */
    public class NullLexusDbSchemaMigrator : ILexusDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}