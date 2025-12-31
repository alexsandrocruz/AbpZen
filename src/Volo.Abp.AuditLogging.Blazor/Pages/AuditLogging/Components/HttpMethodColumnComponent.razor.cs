using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging.Components;

public partial class HttpMethodColumnComponent : AbpComponentBase
{
    [Parameter]
    public object Data { get; set; }

    protected virtual string GetHttpStatusCodeText(HttpStatusCode statusCode)
    {
        return $"{(int)statusCode} - {Regex.Replace(statusCode.ToString(), "([A-Z])", " $1")}";
    }

    protected virtual Color GetHttpStatusCodeBadgeColor(int? statusCode)
    {
        if (statusCode is null)
        {
            return Color.Primary;
        }

        if (statusCode >= 200 && statusCode < 300)
        {
            return Color.Success;
        }

        if (statusCode >= 300 && statusCode < 400)
        {
            return Color.Warning;
        }

        if (statusCode >= 400 && statusCode < 600)
        {
            return Color.Danger;
        }

        return Color.Primary;
    }

    protected virtual Color GetHttpMethodBadgeColor(string method)
    {
        return method switch
        {
            "GET" => Color.Info,
            "POST" => Color.Success,
            "DELETE" => Color.Danger,
            "PUT" => Color.Warning,
            _ => Color.Default
        };
    }
}
