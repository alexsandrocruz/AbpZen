using System.ComponentModel.DataAnnotations;

namespace Volo.Payment.Iyzico;

public class IyzicoAddress
{
    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ZipCode { get; set; }
}
