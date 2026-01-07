using Volo.Abp.Settings;

namespace Sapienza.Zen.Settings
{
    public class SapienzaZenSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(SapienzaZenSettings.MySetting1));
        }
    }
}
