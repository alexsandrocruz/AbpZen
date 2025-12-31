using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.FileManagement.Directories;

public class InvalidDirectoryNameException : BusinessException
{
    public InvalidDirectoryNameException([NotNull] string directoryName)
    {
        Code = FileManagementErrorCodes.InvalidDirectoryName;
        WithData("DirectoryName", directoryName);
    }
}
