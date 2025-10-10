using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            // Booking → BookingDto
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityOccurrence.ActivityId))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.ActivityOccurrence.Activity.Name))
                .ForMember(dest => dest.ActivityDescription, opt => opt.MapFrom(src => src.ActivityOccurrence.Activity.Description))
                .ForMember(dest => dest.ActivityPrice, opt => opt.MapFrom(src => src.ActivityOccurrence.Activity.Price))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.ActivityOccurrence.Zone.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ActivityOccurrence.Activity.Category.Name));

            // Booking → CreatedBookingDto
            CreateMap<Booking, CreatedBookingDto>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityOccurrence.ActivityId))
                .ForMember(dest => dest.ActivityOccurrenceId, opt => opt.MapFrom(src => src.ActivityOccurrenceId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ActivityOccurrence.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.ActivityOccurrence.EndTime));

            // CreateBookingDto → Booking
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // sätts i service
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // sätts i service
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
