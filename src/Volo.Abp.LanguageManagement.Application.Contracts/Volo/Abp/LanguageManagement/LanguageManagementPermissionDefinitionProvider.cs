using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.LanguageManagement;

public class LanguageManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var languageManagementGroup = context.AddGroup(LanguageManagementPermissions.GroupName, L("Permission:LanguageManagement"));

        var textGroup = languageManagementGroup.AddPermission(LanguageManagementPermissions.LanguageTexts.Default, L("Permission:LanguageTexts"))
            .RequireFeatures(LanguageManagementFeatures.Enable);

        textGroup.AddChild(LanguageManagementPermissions.LanguageTexts.Edit, L("Permission:LanguageTextsEdit"))
            .RequireFeatures(LanguageManagementFeatures.Enable);

        var langGroup = languageManagementGroup.AddPermission(LanguageManagementPermissions.Languages.Default, L("Permission:Languages"))
            .RequireFeatures(LanguageManagementFeatures.Enable);

        langGroup.AddChild(LanguageManagementPermissions.Languages.Create, L("Permission:LanguagesCreate"), multiTenancySide: MultiTenancySides.Host)
            .RequireFeatures(LanguageManagementFeatures.Enable);
        langGroup.AddChild(LanguageManagementPermissions.Languages.Edit, L("Permission:LanguagesEdit"), multiTenancySide: MultiTenancySides.Host)
            .RequireFeatures(LanguageManagementFeatures.Enable);
        langGroup.AddChild(LanguageManagementPermissions.Languages.ChangeDefault, L("Permission:LanguagesChangeDefault"))
            .RequireFeatures(LanguageManagementFeatures.Enable);
        langGroup.AddChild(LanguageManagementPermissions.Languages.Delete, L("Permission:LanguagesDelete"), multiTenancySide: MultiTenancySides.Host)
            .RequireFeatures(LanguageManagementFeatures.Enable);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LanguageManagementResource>(name);
    }
}
