using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Booking.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Booking;

public interface IBookingAppService :
    ICrudAppService<
        BookingDto,
        Guid,
        BookingGetListInput,
        CreateUpdateBookingDto,
        CreateUpdateBookingDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetBookingLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
