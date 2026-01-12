using Sapienza.Lexus.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Sapienza.Lexus.Permissions
{
    public class LexusPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(LexusPermissions.GroupName);

            myGroup.AddPermission(LexusPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(LexusPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(LexusPermissions.MyPermission1, L("Permission:MyPermission1"));
                        var lawyerPermission = myGroup.AddPermission(LawyerPermissions.Default, L("Permission:Lawyer"));
            lawyerPermission.AddChild(LawyerPermissions.Create, L("Permission:Create"));
            lawyerPermission.AddChild(LawyerPermissions.Update, L("Permission:Update"));
            lawyerPermission.AddChild(LawyerPermissions.Delete, L("Permission:Delete"));
                  var casePermission = myGroup.AddPermission(CasePermissions.Default, L("Permission:Case"));
            casePermission.AddChild(CasePermissions.Create, L("Permission:Create"));
            casePermission.AddChild(CasePermissions.Update, L("Permission:Update"));
            casePermission.AddChild(CasePermissions.Delete, L("Permission:Delete"));
                  var clientPermission = myGroup.AddPermission(ClientPermissions.Default, L("Permission:Client"));
            clientPermission.AddChild(ClientPermissions.Create, L("Permission:Create"));
            clientPermission.AddChild(ClientPermissions.Update, L("Permission:Update"));
            clientPermission.AddChild(ClientPermissions.Delete, L("Permission:Delete"));
                  var specializationPermission = myGroup.AddPermission(SpecializationPermissions.Default, L("Permission:Specialization"));
            specializationPermission.AddChild(SpecializationPermissions.Create, L("Permission:Create"));
            specializationPermission.AddChild(SpecializationPermissions.Update, L("Permission:Update"));
            specializationPermission.AddChild(SpecializationPermissions.Delete, L("Permission:Delete"));
                  var legalProcessPermission = myGroup.AddPermission(LegalProcessPermissions.Default, L("Permission:LegalProcess"));
            legalProcessPermission.AddChild(LegalProcessPermissions.Create, L("Permission:Create"));
            legalProcessPermission.AddChild(LegalProcessPermissions.Update, L("Permission:Update"));
            legalProcessPermission.AddChild(LegalProcessPermissions.Delete, L("Permission:Delete"));
                  var lawyerSpecializationPermission = myGroup.AddPermission(LawyerSpecializationPermissions.Default, L("Permission:LawyerSpecialization"));
            lawyerSpecializationPermission.AddChild(LawyerSpecializationPermissions.Create, L("Permission:Create"));
            lawyerSpecializationPermission.AddChild(LawyerSpecializationPermissions.Update, L("Permission:Update"));
            lawyerSpecializationPermission.AddChild(LawyerSpecializationPermissions.Delete, L("Permission:Delete"));
                  var proposalPermission = myGroup.AddPermission(ProposalPermissions.Default, L("Permission:Proposal"));
            proposalPermission.AddChild(ProposalPermissions.Create, L("Permission:Create"));
            proposalPermission.AddChild(ProposalPermissions.Update, L("Permission:Update"));
            proposalPermission.AddChild(ProposalPermissions.Delete, L("Permission:Delete"));
                  var propostalItemPermission = myGroup.AddPermission(PropostalItemPermissions.Default, L("Permission:PropostalItem"));
            propostalItemPermission.AddChild(PropostalItemPermissions.Create, L("Permission:Create"));
            propostalItemPermission.AddChild(PropostalItemPermissions.Update, L("Permission:Update"));
            propostalItemPermission.AddChild(PropostalItemPermissions.Delete, L("Permission:Delete"));
      // <ZenCode-PermissionDefinition-Marker>
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LexusResource>(name);
        }
    }
}