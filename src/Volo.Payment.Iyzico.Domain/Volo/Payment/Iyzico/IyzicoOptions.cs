using System.Collections.Generic;

namespace Volo.Payment.Iyzico;

public class IyzicoOptions
{
    public string ApiKey { get; set; }

    public string SecretKey { get; set; }

    public string BaseUrl { get; set; }

    public string Currency { get; set; }

    public string Locale { get; set; }

    public int InstallmentCount { get; set; }
}
