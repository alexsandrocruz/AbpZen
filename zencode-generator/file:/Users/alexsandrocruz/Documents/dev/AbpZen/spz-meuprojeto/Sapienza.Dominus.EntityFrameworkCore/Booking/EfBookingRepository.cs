using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Booking;

public class EfBookingRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Booking.Booking, Guid>, 
      IBookingRepository
{
    public EfBookingRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
