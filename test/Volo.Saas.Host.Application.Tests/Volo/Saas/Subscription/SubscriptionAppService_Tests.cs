using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Payment.Plans;
using Volo.Payment.Requests;
using Volo.Saas.Editions;
using Volo.Saas.Host;
using Xunit;

namespace Volo.Saas.Subscription;

public class SubscriptionAppService_Tests : SaasHostApplicationTestBase
{
    private readonly SaasTestData saasTestData;
    private readonly ISubscriptionAppService subscriptionAppService;
    private IPaymentRequestAppService paymentRequestAppService;

    public SubscriptionAppService_Tests()
    {
        saasTestData = GetRequiredService<SaasTestData>();
        subscriptionAppService = GetRequiredService<ISubscriptionAppService>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        paymentRequestAppService = NSubstitute.Substitute.For<IPaymentRequestAppService>();

        services.AddSingleton(paymentRequestAppService);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ShouldWorkProperly()
    {
        var returnDto = new PaymentRequestWithDetailsDto { Id = Guid.NewGuid()};

        paymentRequestAppService.CreateAsync(null).ReturnsForAnyArgs(returnDto);

        var paymentRequest = await subscriptionAppService.CreateSubscriptionAsync(saasTestData.FirstEditionId, saasTestData.FirstTenantId);

        paymentRequest.ShouldNotBeNull();
        paymentRequest.Id.ShouldBe(returnDto.Id);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ShouldThrowException_ForEditionWithoutPlanId()
    {
        var exception = await Should.ThrowAsync<EditionDoesntHavePlanException>(
            subscriptionAppService.CreateSubscriptionAsync(saasTestData.SecondEditionId, saasTestData.FirstTenantId));

        exception.ShouldNotBeNull();
        exception.EditionId.ShouldBe(saasTestData.SecondEditionId);
    }
}
