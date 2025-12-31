using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Newsletters.Helpers;

public class SecurityCodeProvider : ITransientDependency
{
    public static string Salt { private get; set; } = "this-is-a-abp-newsletter-key";

    public virtual string GetSecurityCode(string emailAddress)
    {
        using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(Salt)))
        {
            var securityCode = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));

            return string.Concat(securityCode.Select(x => x.ToString("x2")));
        }
    }
}
