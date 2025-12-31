using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Account.Public.Web.Pages.Account;

[Authorize]
public class ProtocolCallbackModel : AccountPageModel
{
    public string RedirectUri { get; set; }

    public virtual Task<IActionResult> OnGetAsync(string protocol)
    {
        if (protocol.IsNullOrEmpty())
        {
            Logger.LogWarning("Protocol name is not specified!");
            return Task.FromResult<IActionResult>(RedirectToPage("/Account/Login"));
        }

        RedirectUri = protocol + "://?" + Request.Query
            .Select(q => $"{q.Key}={UrlEncoder.Default.Encode(q.Value)}")
            .JoinAsString("&");

        return Task.FromResult<IActionResult>(Page());
    }
}
