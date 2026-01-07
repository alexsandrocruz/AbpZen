using Volo.Abp.Settings;

namespace Sapienza.Cursos.Settings
{
    public class CursosSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(CursosSettings.MySetting1));
        }
    }
}
