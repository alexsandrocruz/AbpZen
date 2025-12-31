using System;
using Volo.Abp;

namespace Volo.Forms.Responses;

public class ResponseNotEditableException : BusinessException
{
    public ResponseNotEditableException()
    {
        Code = FormsErrorCodes.ResponseNotEditable;
    }
}
