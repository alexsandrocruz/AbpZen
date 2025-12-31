# File Management

This module is used to upload, download and organize files in a hierarchical folder structure. It is also compatible to multi-tenancy and you can determine total size limit for your tenants.

## Permissions

After the module installation, the following permissions are added to the system:

- **FileManagement**
  - **FileManagement.DirectoryDescriptor**
    - **FileManagement.DirectoryDescriptor.Create**
    - **FileManagement.DirectoryDescriptor.Update**
    - **FileManagement.DirectoryDescriptor.Delete**
  - **FileManagement.FileDescriptor**
    - **FileManagement.FileDescriptor.Create**
    - **FileManagement.FileDescriptor.Update**
    - **FileManagement.FileDescriptor.Delete**

You may need to give these permissions to the roles that you want to allow to access the File Management UI.

## Documentation

For more information, see the [module documentation](https://abp.io/docs/latest/modules/file-management).
