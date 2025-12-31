namespace Volo.Abp.Account;

public class NoImageProvidedException : BusinessException
{
    public NoImageProvidedException()
    {
        Code = AccountProErrorCodes.NoImageProvided;
    }
}
