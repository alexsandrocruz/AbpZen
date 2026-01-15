using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Booking;

public interface IBookingRepository : IRepository<Sapienza.Dominus.Booking.Booking, Guid>
{
}
