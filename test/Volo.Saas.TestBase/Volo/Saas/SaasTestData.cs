using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Saas;

public class SaasTestData : ISingletonDependency
{
    public Guid FirstEditionId { get; } = Guid.NewGuid();

    public Guid SecondEditionId { get; } = Guid.NewGuid();

    public Guid FirstTenantId { get; internal set; }

    public string FirstTenantName { get; } = "acme";

    public Guid SecondTenantId { get; internal set; }

    public string SecondTenantName { get; } = "volosoft";

    public Guid FirstPlanId { get; } = Guid.NewGuid();

    public string FirstPlanName { get; } = "Pro Plan";

    public DateTime FirstTenantExpireDate { get; } = DateTime.Now.AddYears(1);
}
