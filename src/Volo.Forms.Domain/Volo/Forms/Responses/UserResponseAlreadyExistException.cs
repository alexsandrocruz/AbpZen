using System;
using Volo.Abp;

namespace Volo.Forms.Responses;

public class UserResponseAlreadyExistException : BusinessException
{
    public UserResponseAlreadyExistException()
    {
        Code = FormsErrorCodes.UserResponseAlreadyExist;
    }
}
