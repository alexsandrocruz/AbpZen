using System;
using Volo.Abp;

namespace Volo.FileManagement.Directories;

public class InvalidMoveException : BusinessException
{
    public InvalidMoveException()
    {
        Code = FileManagementErrorCodes.CantMovableToUnderChild;
    }
}
