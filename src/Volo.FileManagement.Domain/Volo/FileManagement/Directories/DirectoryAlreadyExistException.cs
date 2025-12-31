using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.FileManagement.Directories;

public class DirectoryAlreadyExistException : BusinessException
{
    public DirectoryAlreadyExistException([NotNull] string directoryName)
    {
        Code = FileManagementErrorCodes.DirectoryAlreadyExist;
        WithData("DirectoryName", directoryName);
    }
}
