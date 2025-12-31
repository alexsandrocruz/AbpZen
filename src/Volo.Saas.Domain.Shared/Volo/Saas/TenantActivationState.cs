namespace Volo.Saas;

public enum TenantActivationState : byte
{
    Active = 0,

    ActiveWithLimitedTime = 1,

    Passive = 2
}
