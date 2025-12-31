namespace Volo.Abp.Account.AuthorityDelegation;

public class AbpAccountAuthorityDelegationOptions
{
    public bool EnableDelegatedImpersonation { get; set; }

    public AbpAccountAuthorityDelegationOptions()
    {
        EnableDelegatedImpersonation = true;
    }
}