# Audit Logging Pro

This module provides the audit log reporting UI for the auditing infrastructure. Allows to search, filter and show audit log entries and entity change logs.

## Audit Logging Infrastructure

The audit logging infrastructure is provided by the `Volo.Abp.AuditLogging` module and it is not a part of the `Volo.Abp.AuditLogging.Pro` module. If the domain or infrastructure layer of your application doesn't have the required `Volo.Abp.AuditLogging` packages, you must add them yourself:

- **Volo.Abp.AuditLogging.Domain** (*AbpAuditLoggingDomainModule*)
- **Volo.Abp.AuditLogging.Domain.Shared** (*AbpAuditLoggingDomainSharedModule*)
- **Volo.Abp.AuditLogging.EntityFrameworkCore** (*AbpAuditLoggingEntityFrameworkCoreModule*)
- **Volo.Abp.AuditLogging.MongoDB** (*AbpAuditLoggingMongoDbModule*)

### EntityFramework Core Configuration

For `EntityFrameworkCore`, the further configuration will be needed in the `OnModelCreating` method of your `DbContext` class:

````csharp
using Volo.Abp.AuditLogging.EntityFrameworkCore;
	
	//...

	protected override void OnModelCreating(ModelBuilder builder)
	{
	        base.OnModelCreating(builder);
	
	        builder.ConfigureAuditLogging();
	        
	        //...
	}
````

Also, you will need to create a new migration and apply it to the database.

## Permissions

After the module installation, the following permissions are added to the system:

- **AuditLogging.AuditLogs**
  - **AuditLogging.AuditLogs.SettingManagement**

You may need to give these permissions to the roles that you want to allow to access the audit log reporting UI.

## Documentation

For more information, see the [module documentation](https://abp.io/docs/latest/modules/audit-logging-pro).
