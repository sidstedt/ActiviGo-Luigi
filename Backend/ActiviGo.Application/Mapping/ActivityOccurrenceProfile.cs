using AutoMapper;
using ActiviGo.Domain.Models;
using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Enum;

namespace ActiviGo.Application.Mapping
{
    public class ActivityOccurrenceProfile : Profile
    {
        public ActivityOccurrenceProfile() //Mappings for ActivityOccurrence
        {
            CreateMap<ActivityOccurrenceCreateDto, ActivityOccurrence>();
            CreateMap<ActivityOccurrenceUpdateDto, ActivityOccurrence>();

            CreateMap<ActivityOccurrence, ActivityOccurrenceResponseDto>()
                .ForMember(
                    dest => dest.ActivityName,
                    opt => opt.MapFrom(src => src.Activity.Name)
                )
                .ForMember(
                    dest => dest.ZoneName,
                    opt => opt.MapFrom(src => src.Zone.Name)
                )
                .ForMember(
                    dest => dest.IsOutdoor,
                    opt => opt.MapFrom(src => src.Zone.IsOutdoor)
                )
                .ForMember(
                    dest => dest.Latitude,
                    opt => opt.MapFrom(src => (double?)src.Zone.Location.Latitude)
                )
                .ForMember(
                    dest => dest.Longitude,
                    opt => opt.MapFrom(src => (double?)src.Zone.Location.Longitude)
                )
                .ForMember(
                    dest => dest.MaxCapacity,
                    opt => opt.MapFrom(src => src.Activity.MaxParticipants)
                )
                .ForMember(
                    dest => dest.ParticipantsCount,
                    opt => opt.MapFrom(src => src.Bookings.Count(b => b.Status != BookingStatus.Canceled))
                )
                .ForMember(
                    dest => dest.AvailableSlots,
                    opt => opt.MapFrom(src => src.Activity.MaxParticipants - src.Bookings.Count(b => b.Status != BookingStatus.Canceled))
                );
        }
    }
}