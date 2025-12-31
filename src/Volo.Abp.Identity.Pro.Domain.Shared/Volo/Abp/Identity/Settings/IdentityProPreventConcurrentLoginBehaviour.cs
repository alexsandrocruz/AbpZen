namespace Volo.Abp.Identity.Settings;

public enum IdentityProPreventConcurrentLoginBehaviour
{
    Disabled = 0,

    LogoutFromSameTypeDevices = 1,

    LogoutFromAllDevices = 2
}
