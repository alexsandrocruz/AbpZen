using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.FileManagement.Files;

public class FileAlreadyExistException : BusinessException
{
    public FileAlreadyExistException([NotNull] string fileName)
    {
        Code = FileManagementErrorCodes.FileAlreadyExist;
        WithData("FileName", fileName);
    }
}
