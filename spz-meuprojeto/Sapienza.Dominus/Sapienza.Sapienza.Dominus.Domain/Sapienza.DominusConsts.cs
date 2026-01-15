using Volo.Abp.Identity;

namespace Sapienza.Dominus
{
    public static class DominusConsts
    {
        public const string DbTablePrefix = "App";
        public const string DbSchema = null;
        public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
        public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
    }
}
