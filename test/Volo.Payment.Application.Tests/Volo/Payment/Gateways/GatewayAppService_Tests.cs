using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Payment.Gateways;

public class GatewayAppService_Tests : PaymentTestBase<PaymentApplicationTestModule>
{
    private IGatewayAppService gatewayAppService;

    public GatewayAppService_Tests()
    {
        gatewayAppService = GetRequiredService<IGatewayAppService>();
    }

    [Fact]
    public async Task GetGatewayConfigurationAsync_ShouldWorkProperly()
    {
        var gatewayConfiguration = await gatewayAppService.GetGatewayConfigurationAsync();

        gatewayConfiguration.ShouldNotBeNull();
    }
}
