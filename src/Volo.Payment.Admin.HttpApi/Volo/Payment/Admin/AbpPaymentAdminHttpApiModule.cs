using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Payment.Admin;

[DependsOn(
    typeof(AbpPaymentHttpApiModule),
    typeof(AbpPaymentAdminApplicationContractsModule)
    )]
public class AbpPaymentAdminHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPaymentAdminHttpApiModule).Assembly);
        });
    }
}
