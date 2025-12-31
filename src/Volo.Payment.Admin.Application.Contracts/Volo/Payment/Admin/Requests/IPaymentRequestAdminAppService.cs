using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Requests;

public interface IPaymentRequestAdminAppService : IApplicationService
{
    Task<PaymentRequestWithDetailsDto> GetAsync(Guid id);
    Task<PagedResultDto<PaymentRequestWithDetailsDto>> GetListAsync(PaymentRequestGetListInput input);
}
