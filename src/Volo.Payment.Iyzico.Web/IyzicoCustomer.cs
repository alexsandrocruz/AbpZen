using System.ComponentModel.DataAnnotations;

namespace Volo.Payment.Iyzico;

public class IyzicoCustomer
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string IdentityNumber { get; set; }

    public string IpAddress { get; set; }

    public IyzicoAddress Address { get; set; }

    public IyzicoCustomer()
    {
        IdentityNumber = "22222222222";
        Address = new IyzicoAddress();
    }
}
