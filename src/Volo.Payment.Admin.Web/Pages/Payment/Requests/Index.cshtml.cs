using System;
using System.ComponentModel.DataAnnotations;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Web.Pages.Payment.Requests;

public class IndexModel : PaymentPageModel
{
    [Display(Name = "PaymentRequests:Search")]
    public string Filter { get; set; }
	
    [DataType(DataType.Date)]
    public DateTime? CreationDateMax { get; set; }
	
    [DataType(DataType.Date)]
    public DateTime? CreationDateMin { get; set; }

    [Display(Name = "PaymentRequests:PaymentType")]
    public PaymentType? PaymentType { get; set; }

    [Display(Name = "PaymentRequests:Status")]
    public PaymentRequestState? Status { get; set; }
}
