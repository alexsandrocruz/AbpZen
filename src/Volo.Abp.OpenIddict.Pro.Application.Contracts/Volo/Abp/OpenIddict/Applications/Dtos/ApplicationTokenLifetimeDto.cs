namespace Volo.Abp.OpenIddict.Applications.Dtos;

public class ApplicationTokenLifetimeDto
{
    public double? AccessTokenLifetime { get; set; }

    public double? AuthorizationCodeLifetime  { get; set; }

    public double? DeviceCodeLifetime  { get; set; }

    public double? IdentityTokenLifetime { get; set; }

    public double? RefreshTokenLifetime { get; set; }

    public double? UserCodeLifetime { get; set; }
}
