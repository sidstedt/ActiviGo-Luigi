using AutoMapper;
using ActiviGo.Domain.Models;
using ActiviGo.Application.DTOs;

namespace ActiviGo.Application.Mapping
{
    public class ActivityOccurrenceProfile : Profile
    {
        public ActivityOccurrenceProfile()
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
                    dest => dest.MaxCapacity,
                    opt => opt.MapFrom(src => src.Activity.MaxParticipants)
                )
                .ForMember(
                    dest => dest.ParticipantsCount,
                    opt => opt.MapFrom(src => src.Bookings.Count)
                )
                .ForMember(
                    dest => dest.AvailableSlots,
                    opt => opt.MapFrom(src => src.Activity.MaxParticipants - src.Bookings.Count)
                );
        }
    }
}