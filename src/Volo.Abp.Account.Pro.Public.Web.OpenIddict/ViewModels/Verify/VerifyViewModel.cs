using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Volo.Abp.Account.Web.ViewModels.Verify;

public class VerifyViewModel
{
    public string ApplicationName { get; set; }

    [BindNever]
    public string Error { get; set; }

    [BindNever]
    public string ErrorDescription { get; set; }

    public string Scope { get; set; }

    [FromQuery(Name = "user_code")]
    public string UserCode { get; set; }
}
