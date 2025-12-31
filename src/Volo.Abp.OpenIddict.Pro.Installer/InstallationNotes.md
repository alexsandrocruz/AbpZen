# OpenIddict Pro

This module provides complete integration and management UI for the  OpenIddict  package.

## Permissions

After the module installation, the following permissions are added to the system:

- **OpenIddictPro**
  - **OpenIddictPro.Application**
    - **OpenIddictPro.Application.Create**
    - **OpenIddictPro.Application.Update**
    - **OpenIddictPro.Application.Delete**
    - **OpenIddictPro.Application.ManagePermissions**
  - **OpenIddictPro.Scope**
    - **OpenIddictPro.Scope.Create**
    - **OpenIddictPro.Scope.Update**
    - **OpenIddictPro.Scope.Delete**
- **AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Applications.Application**
- **AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Scopes.Scope**

You may need to give these permissions to the roles that you want to allow to access the OpenIddict UI.

## Documentation

For more information, see the [module documentation](https://abp.io/docs/latest/modules/openiddict-pro).
