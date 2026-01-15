using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Booking;

public interface IBookingRepository : IRepository<Dominus.Booking.Booking, Guid>
{
}
