using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Payment.Gateways;

public interface IGatewayAppService : IApplicationService
{
    Task<List<GatewayDto>> GetGatewayConfigurationAsync();

    Task<List<GatewayDto>> GetSubscriptionSupportedGatewaysAsync();
}
