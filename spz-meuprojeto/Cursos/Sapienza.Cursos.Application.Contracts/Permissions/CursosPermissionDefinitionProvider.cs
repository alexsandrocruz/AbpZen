using Sapienza.Cursos.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.Cursos.Permissions
{
    public class CursosPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(CursosPermissions.GroupName);

            myGroup.AddPermission(CursosPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(CursosPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(CursosPermissions.MyPermission1, L("Permission:MyPermission1"));
            var turmaPermission = myGroup.AddPermission(TurmaPermissions.Default, L("Permission:Turma"));
            turmaPermission.AddChild(TurmaPermissions.Create, L("Permission:Create"));
            turmaPermission.AddChild(TurmaPermissions.Update, L("Permission:Update"));
            turmaPermission.AddChild(TurmaPermissions.Delete, L("Permission:Delete"));
            var cursoPermission = myGroup.AddPermission(CursoPermissions.Default, L("Permission:Curso"));
            cursoPermission.AddChild(CursoPermissions.Create, L("Permission:Create"));
            cursoPermission.AddChild(CursoPermissions.Update, L("Permission:Update"));
            cursoPermission.AddChild(CursoPermissions.Delete, L("Permission:Delete"));
      // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CursosResource>(name);
        }
    }
}