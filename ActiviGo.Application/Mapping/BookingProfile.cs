using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using AutoMapper;

namespace ActiviGo.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            // Booking → BookingDto
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityOccurence.ActivityId))
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.ActivityOccurence.Activity.Name))
                .ForMember(dest => dest.ActivityDescription, opt => opt.MapFrom(src => src.ActivityOccurence.Activity.Description))
                .ForMember(dest => dest.ActivityPrice, opt => opt.MapFrom(src => src.ActivityOccurence.Activity.Price))
                .ForMember(dest => dest.ZoneName, opt => opt.MapFrom(src => src.ActivityOccurence.Zone.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ActivityOccurence.Activity.Category.Name));

            // Booking → CreatedBookingDto
            CreateMap<Booking, CreatedBookingDto>()
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityOccurence.ActivityId))
                .ForMember(dest => dest.ActivityOccurenceId, opt => opt.MapFrom(src => src.ActivityOccurenceId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ActivityOccurence.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.ActivityOccurence.EndTime));

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
