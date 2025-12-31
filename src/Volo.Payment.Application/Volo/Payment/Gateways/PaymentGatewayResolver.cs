using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Payment.Gateways;

public class PaymentGatewayResolver : IScopedDependency
{
    protected PaymentOptions PaymentOptions { get; }

    protected ILogger<PaymentGatewayResolver> Logger { get; }

    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayResolver(
        IOptions<PaymentOptions> paymentOptions,
        ILogger<PaymentGatewayResolver> logger,
        IServiceProvider serviceProvider)
    {
        PaymentOptions = paymentOptions.Value;
        Logger = logger;
        _serviceProvider = serviceProvider;
    }

    public virtual IPaymentGateway Resolve(string gatewayName)
    {
        var gatewayConfiguration = PaymentOptions
            .Gateways
            .FirstOrDefault(x => x.Value.Name.Equals(gatewayName, StringComparison.InvariantCultureIgnoreCase))
                .Value ?? throw new ArgumentException($"Payment gateway with name {gatewayName} not found.", nameof(gatewayName)); ;

        var gateway = _serviceProvider.GetRequiredService(gatewayConfiguration.PaymentGatewayType);

        if (gateway is not IPaymentGateway paymentGateway)
        {
            throw new InvalidOperationException($"'PaymentGatewayType' of PaymentOptions isn't configured properly. That type must implement 'IPaymentGateway'");
        }

        return paymentGateway;
    }
}
