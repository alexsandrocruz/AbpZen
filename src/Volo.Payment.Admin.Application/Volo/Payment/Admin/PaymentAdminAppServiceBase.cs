using Volo.Abp.Application.Services;
using Volo.Payment.Localization;

namespace Volo.Payment.Admin;

public abstract class PaymentAdminAppServiceBase : ApplicationService
{
    protected PaymentAdminAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpPaymentAdminApplicationModule);
        LocalizationResource = typeof(PaymentResource);
    }
}
