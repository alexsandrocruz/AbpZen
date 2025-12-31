using Volo.Abp.Data;
using Volo.Forms.Responses;

namespace Volo.Forms;

public static class FormResponseExtensions
{
    private const string EmailPropertyName = "Email";

    public static void SetEmail(this FormResponse response, string email)
    {
        response.SetProperty(EmailPropertyName, email);
    }

    public static string GetEmail(this FormResponse response)
    {
        return response.GetProperty<string>(EmailPropertyName);
    }
}
