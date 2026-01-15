using AutoMapper;
using Sapienza.Dominus.Booking.Dtos;

namespace Sapienza.Dominus.Booking;

public class BookingAutoMapperProfile : Profile
{
    public BookingAutoMapperProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.SchedulerTypeDisplayName, opt => opt.MapFrom(src => src.SchedulerType.Name))
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateBookingDto, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateBookingDto, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
