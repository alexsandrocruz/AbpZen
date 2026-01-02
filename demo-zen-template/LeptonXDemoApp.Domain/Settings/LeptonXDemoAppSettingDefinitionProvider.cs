using Volo.Abp.Settings;

namespace LeptonXDemoApp.Settings
{
    public class LeptonXDemoAppSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(LeptonXDemoAppSettings.MySetting1));
        }
    }
}
