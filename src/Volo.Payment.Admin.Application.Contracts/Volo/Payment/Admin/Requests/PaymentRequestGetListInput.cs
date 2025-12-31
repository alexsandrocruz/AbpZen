using System;
using Volo.Abp.Application.Dtos;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Requests;

public class PaymentRequestGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public DateTime? CreationDateMax { get; set; }

    public DateTime? CreationDateMin { get; set; }

    public PaymentType? PaymentType { get; set; }

    public PaymentRequestState? Status { get; set; }
}
