namespace Volo.Saas;

public static class SaasErrorCodes
{
    internal const string Root = "Saas";
    public static class Edition
    {
        internal const string Group = Root + ":Edition";

        public const string EditionDoesntHavePlan = Group + ":0001";

        public const string UnableDeleteEdition = Group + ":0002";
    }
}
