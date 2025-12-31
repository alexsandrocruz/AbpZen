using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Saas.Host;
using Xunit;

namespace Volo.Saas.Tenant;

public class EditionAppService_Tests : SaasTenantApplicationTestBase
{
    protected IEditionAppService EditionAppService { get; }

    public EditionAppService_Tests()
    {
        EditionAppService = GetRequiredService<IEditionAppService>();
    }

    [Fact]
    public async Task GetPlanLookupAsync_ShouldWorkProperly()
    {
        var plans = await EditionAppService.GetPlanLookupAsync();

        plans.ShouldNotBeNull();
    }
}
