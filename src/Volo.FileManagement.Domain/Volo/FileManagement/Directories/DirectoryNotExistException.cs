using System;
using Volo.Abp;

namespace Volo.FileManagement.Directories;

public class DirectoryNotExistException : BusinessException
{
    public DirectoryNotExistException()
    {
        Code = FileManagementErrorCodes.DirectoryNotExist;
    }
}
