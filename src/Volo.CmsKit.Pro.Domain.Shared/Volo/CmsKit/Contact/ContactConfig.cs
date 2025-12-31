namespace Volo.CmsKit.Contact;
public class ContactConfig
{
    public string ReceiverEmailAddress { get; }

    public ContactConfig(string receiverEmailAddress)
    {
        ReceiverEmailAddress = receiverEmailAddress;
    }
}
