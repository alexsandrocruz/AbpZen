namespace Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

public class LoginResult
{
    public bool IsError { get; set; }

    public string Error { get; set; }

    public string ErrorDescription { get; set; }

    public static LoginResult Success()
    {
        return new LoginResult
        {
            IsError = false
        };
    }

    public static LoginResult Failed(string error, string errorDescription)
    {
        return new LoginResult
        {
            IsError = true,
            Error = error,
            ErrorDescription = errorDescription
        };
    }
}
