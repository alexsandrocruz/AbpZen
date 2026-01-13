using Volo.Abp.Identity;

namespace Sapienza.EventoZen
{
    public static class EventoZenConsts
    {
        public const string DbTablePrefix = "App";
        public const string DbSchema = null;
        public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
        public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
    }
}
