using Volo.Abp.Settings;

namespace Sapienza.EventoZen.Settings
{
    public class EventoZenSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(EventoZenSettings.MySetting1));
        }
    }
}
