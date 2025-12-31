using System.Collections.Generic;
using Volo.Abp.Account.ExternalProviders;

namespace Volo.Abp.Account;

public class AccountExternalProviderSettingsDto
{
    public bool VerifyPasswordDuringExternalLogin { get; set; }

    public List<ExternalProviderSettings> ExternalProviders { get; set; }
}
