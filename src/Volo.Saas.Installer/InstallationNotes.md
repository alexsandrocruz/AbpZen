# SaaS

This module is used to create Software as a Service (SaaS) applications based on a multi-tenant architecture.

## Enabling Multi-Tenancy

If you are going to use SaaS module, make sure that multi-tenancy is enabled in your application. See the [multi-tenancy documentation](https://docs.abp.io/en/abp/latest/Multi-Tenancy) for more information.

> In ABP Studio templates, multi tenancy can be enabled/disabled easily in a single point. 

## Permissions

After the module installation, the following permissions are added to the system:

- **Saas**
  - **Saas.Tenants**
    - **Saas.Tenants.Create**
    - **Saas.Tenants.Update**
    - **Saas.Tenants.Delete**
    - **Saas.Tenants.ManageFeatures**
    - **Saas.Tenants.ManageConnectionStrings**
    - **Saas.Tenants.SetPassword**
    - **Saas.Tenants.Impersonation**
  - **Saas.Editions**
    - **Saas.Editions.Create**
    - **Saas.Editions.Update**
    - **Saas.Editions.Delete**
    - **Saas.Editions.ManageFeatures**
- **AuditLogging.ViewChangeHistory:Volo.Saas.Tenant**
- **AuditLogging.ViewChangeHistory:Volo.Saas.Edition**

You may need to give these permissions to the roles that you want to allow to access the SaaS UI.

## Documentation

For more information, see the [module documentation](https://abp.io/docs/latest/modules/saas).
