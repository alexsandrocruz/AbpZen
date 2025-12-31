using Volo.Abp.Modularity;

namespace Volo.Forms;

[DependsOn(
    typeof(FormsApplicationModule),
    typeof(FormsDomainTestModule)
    )]
public class FormsApplicationTestModule : AbpModule
{

}
