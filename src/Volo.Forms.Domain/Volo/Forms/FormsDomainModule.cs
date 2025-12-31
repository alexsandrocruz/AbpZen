using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Forms;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(FormsDomainSharedModule)
)]
public class FormsDomainModule : AbpModule
{

}
