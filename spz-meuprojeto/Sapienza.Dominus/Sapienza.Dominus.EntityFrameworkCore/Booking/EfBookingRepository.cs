using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Booking;

public class EfBookingRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Booking.Booking, Guid>, 
      IBookingRepository
{
    public EfBookingRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
