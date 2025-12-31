using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Payment.Requests;
using Xunit;

namespace Volo.Payment;

public class PaymentRequestAppService_Tests : PaymentTestBase<PaymentApplicationTestModule>
{
    private readonly IPaymentRequestAppService _paymentRequestAppService;
    private readonly PaymentTestData _testData;

    public PaymentRequestAppService_Tests()
    {
        _paymentRequestAppService = GetRequiredService<IPaymentRequestAppService>();
        _testData = GetRequiredService<PaymentTestData>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var paymentRequestDto = await CreateTestPaymentRequestAsync();

        paymentRequestDto.State.ShouldBe(PaymentRequestState.Waiting);
        paymentRequestDto.Products.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetAsync()
    {
        var paymentRequestDto = await CreateTestPaymentRequestAsync();

        var paymentRequest = await _paymentRequestAppService.GetAsync(paymentRequestDto.Id);
        paymentRequest.ShouldNotBeNull();
    }

    [Fact]
    public async Task StartAsync_ShouldGenerateLink()
    {
        var path = "/custom/return/url";

        var result = await _paymentRequestAppService.StartAsync("Test", new PaymentRequestStartDto
        {
            PaymentRequestId = _testData.PaymentRequest_1_Id,
            ReturnUrl = path,
            CancelUrl = "/",
        });

        result.ShouldNotBeNull();

        // See TestPaymentGateway implementation for following line:
        result.CheckoutLink.ShouldBe(path);
    }

    [Fact]
    public async Task CompleteAsync()
    {
        var paymentRequest = await _paymentRequestAppService.CompleteAsync("Test", new Dictionary<string, string>());

        paymentRequest.State.ShouldBe(PaymentRequestState.Completed);
    }

    private async Task<PaymentRequestWithDetailsDto> CreateTestPaymentRequestAsync()
    {
        return await _paymentRequestAppService.CreateAsync(new PaymentRequestCreateDto()
        {
            Currency = "USD",
            Products = new List<PaymentRequestProductCreateDto>
                {
                    new PaymentRequestProductCreateDto
                    {
                        Code = "001",
                        Count = 1,
                        Name = "Abp Commercial",
                        UnitPrice = 100,
                        TotalPrice = 100,
                        ExtraProperties = new Dictionary<string, IPaymentRequestProductExtraParameterConfiguration>
                        {
                            { "PayPal", new TestGatewayPaymentRequestProductExtraParameterConfiguration() }
                        }
                    }
                }
        });
    }

    internal class TestGatewayPaymentRequestProductExtraParameterConfiguration : IPaymentRequestProductExtraParameterConfiguration
    {

    }
}
