using Volo.Abp.Application.Dtos;

namespace Volo.Payment.Gateways;

public class GatewayDto : EntityDto
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
}
