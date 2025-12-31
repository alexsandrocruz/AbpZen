using System.Collections.Generic;

namespace Volo.Abp.Account;

public class VerifyAuthenticatorCodeDto
{
    public List<string> RecoveryCodes { get; set; }
}
