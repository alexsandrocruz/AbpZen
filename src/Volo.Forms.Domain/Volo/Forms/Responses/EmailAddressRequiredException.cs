using System;
using Volo.Abp;

namespace Volo.Forms.Responses;

public class EmailAddressRequiredException : BusinessException
{
    public EmailAddressRequiredException()
    {
        Code = FormsErrorCodes.EmailAddressRequired;
    }
}
