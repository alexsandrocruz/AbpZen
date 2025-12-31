using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Requests;

[RemoteService(Name = AbpPaymentAdminRemoteServiceConsts.RemoteServiceName)]
[Area(AbpPaymentAdminRemoteServiceConsts.ModuleName)]
[Authorize(PaymentAdminPermissions.PaymentRequests.Default)]
[Route("api/payment-admin/payment-requests")]
public class PaymentRequestAdminController : PaymentAdminController, IPaymentRequestAdminAppService
{
    protected IPaymentRequestAdminAppService PaymentRequestAdminAppService { get; }

    public PaymentRequestAdminController(IPaymentRequestAdminAppService paymentRequestAdminAppService)
    {
        PaymentRequestAdminAppService = paymentRequestAdminAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<PaymentRequestWithDetailsDto>> GetListAsync(PaymentRequestGetListInput input)
    {
        return PaymentRequestAdminAppService.GetListAsync(input);
    }

    [Route("{id}")]
    [HttpGet]
    public virtual Task<PaymentRequestWithDetailsDto> GetAsync(Guid id)
    {
        return PaymentRequestAdminAppService.GetAsync(id);
    }
}
