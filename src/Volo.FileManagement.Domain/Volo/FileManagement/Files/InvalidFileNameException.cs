using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.FileManagement.Files;

public class InvalidFileNameException : BusinessException
{
    public InvalidFileNameException([NotNull] string fileName)
    {
        Code = FileManagementErrorCodes.InvalidFileName;
        WithData("FileName", fileName);
    }
}
