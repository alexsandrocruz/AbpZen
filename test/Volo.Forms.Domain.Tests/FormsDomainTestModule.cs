using Volo.Forms.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Forms;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(FormsEntityFrameworkCoreTestModule)
    )]
public class FormsDomainTestModule : AbpModule
{

}
