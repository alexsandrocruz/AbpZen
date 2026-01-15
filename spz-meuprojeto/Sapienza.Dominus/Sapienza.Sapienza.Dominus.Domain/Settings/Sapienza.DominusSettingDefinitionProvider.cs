using Volo.Abp.Settings;

namespace Sapienza.Dominus.Settings
{
    public class DominusSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DominusSettings.MySetting1));
        }
    }
}
