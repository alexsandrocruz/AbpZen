using Volo.Abp.Settings;

namespace Sapienza.Lexus.Settings
{
    public class LexusSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(LexusSettings.MySetting1));
        }
    }
}
