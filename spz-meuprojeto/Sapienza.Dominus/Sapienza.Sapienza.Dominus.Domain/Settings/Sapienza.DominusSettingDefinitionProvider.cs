using Volo.Abp.Settings;

namespace Sapienza.Sapienza.Dominus.Settings
{
    public class Sapienza.DominusSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(Sapienza.DominusSettings.MySetting1));
        }
    }
}
