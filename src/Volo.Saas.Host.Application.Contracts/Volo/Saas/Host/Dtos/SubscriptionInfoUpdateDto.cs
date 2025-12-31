using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Saas.Host.Dtos;

public class SubscriptionInfoUpdateDto : ExtensibleEntityDto
{
    public Guid EditionId { get; set; }
    public Guid PaymentRequestId { get; set; }
    public DateTime EndDate { get; set; }
}
