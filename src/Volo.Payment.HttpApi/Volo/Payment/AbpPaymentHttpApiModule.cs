using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Payment;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpPaymentApplicationContractsModule)
    )]
public class AbpPaymentHttpApiModule : AbpModule
{

}
