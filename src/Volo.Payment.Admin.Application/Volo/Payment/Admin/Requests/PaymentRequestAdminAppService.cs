using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Requests;

[Authorize(PaymentAdminPermissions.PaymentRequests.Default)]
public class PaymentRequestAdminAppService : PaymentAdminAppServiceBase, IPaymentRequestAdminAppService
{
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    public PaymentRequestAdminAppService(IPaymentRequestRepository paymentRequestRepository)
    {
        PaymentRequestRepository = paymentRequestRepository;
    }

    public virtual async Task<PagedResultDto<PaymentRequestWithDetailsDto>> GetListAsync(PaymentRequestGetListInput input)
    {
        var paymentRequests = await PaymentRequestRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.CreationDateMax,
            input.CreationDateMin,
            input.PaymentType,
            input.Status,
            includeDetails: true);

        return new PagedResultDto<PaymentRequestWithDetailsDto>(
            await PaymentRequestRepository.GetCountAsync(input.Filter),
            ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestWithDetailsDto>>(paymentRequests)
            );
    }

    public virtual async Task<PaymentRequestWithDetailsDto> GetAsync(Guid id)
    {
        var paymentRequest = await PaymentRequestRepository.GetAsync(id);

        return ObjectMapper.Map<PaymentRequest, PaymentRequestWithDetailsDto>(paymentRequest);
    }
}
