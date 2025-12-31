using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Payment.Requests;

public abstract class PaymentRequestRepository_Test<TStartupModule> : PaymentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly PaymentTestData testData;
    private readonly IPaymentRequestRepository paymentRequestRepository;

    public PaymentRequestRepository_Test()
    {
        testData = GetRequiredService<PaymentTestData>();

        paymentRequestRepository = GetRequiredService<IPaymentRequestRepository>();
    }

    [Fact]
    public async Task GetBySubscriptionAsync_ShouldWorkProperly()
    {
        var paymentRequest = await paymentRequestRepository.GetBySubscriptionAsync(testData.PaymentRequest_1_SubscriptionId);

        paymentRequest.ShouldNotBeNull();
        paymentRequest.Id.ShouldBe(testData.PaymentRequest_1_Id);
    }

    [Fact]
    public async Task GetBySubscriptionAsync_ShouldThrowEntityNotFoundException_WithWrongSubscriptionId()
    {
        var nonExistingSubscrtiptionId = "some-wrong-id";
        var exception = await Should.ThrowAsync<EntityNotFoundException>(paymentRequestRepository.GetBySubscriptionAsync(nonExistingSubscrtiptionId));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(typeof(PaymentRequest));
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithoutSortingWithFiltering()
    {
        var paymentRequests = await paymentRequestRepository.GetPagedListAsync(0, 1, sorting: null, filter: null);

        paymentRequests.ShouldNotBeNull();
        paymentRequests.ShouldNotBeEmpty();
        paymentRequests.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WithSortingWithoutFiltering()
    {
        var paymentRequests = await paymentRequestRepository.GetPagedListAsync(0, 2, sorting: nameof(PaymentRequest.ExternalSubscriptionId), filter: null);

        paymentRequests.ShouldNotBeNull();
        paymentRequests.ShouldNotBeEmpty();
        paymentRequests.Count.ShouldBe(2);

        paymentRequests[0].ExternalSubscriptionId.ShouldBeLessThanOrEqualTo(paymentRequests[1].ExternalSubscriptionId);
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnExactNumber_WithFilter()
    {
        var count = await paymentRequestRepository.GetCountAsync(filter: testData.PaymentRequest_1_SubscriptionId);

        count.ShouldBe(1);
    }
}
