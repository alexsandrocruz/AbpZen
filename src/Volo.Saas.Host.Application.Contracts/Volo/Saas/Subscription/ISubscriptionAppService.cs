using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Payment.Requests;

namespace Volo.Saas.Subscription;

public interface ISubscriptionAppService : IApplicationService
{
    Task<PaymentRequestWithDetailsDto> CreateSubscriptionAsync(Guid editionId, Guid tenantId);
}
