using System;
using System.Collections.Generic;
using Shouldly;
using Volo.Payment.Requests;
using Xunit;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutPaymentGateway_Tests : PaymentTestBase<PaymentDomainTestModule>
{
    private readonly TwoCheckoutPaymentGateway _twoCheckoutPaymentGateway;

    public TwoCheckoutPaymentGateway_Tests()
    {
        _twoCheckoutPaymentGateway = GetRequiredService<TwoCheckoutPaymentGateway>();
    }

    [Fact]
    public void TwoCheckoutPaymentGateway_IsValid_Test()
    {
        _twoCheckoutPaymentGateway.IsValid(
            new PaymentRequest(Guid.NewGuid(), "USD") {
                ExtraProperties = {
                    {
                        "ReturnUrl",
                        "http://test.aspnetzero.com/Payment/TwoCheckout/PostPayment?paymentRequestId=f3a73263-4192-7c5a-5deb-39f34c072547&hmac-sha256=f1b0b5f469c4ed6e765a335c06dcdd4b7d338bf97cd5e91a67f13df131124e0a"
                    }
                }
            },
            new Dictionary<string, string> {
                { "hmac-sha256", "f1b0b5f469c4ed6e765a335c06dcdd4b7d338bf97cd5e91a67f13df131124e0a" }
            }).ShouldBeTrue();
    }
}
