using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;

namespace ActiviGo.Application.Interfaces
{
    public interface ILocationService
        : IGenericService<Location, LocationResponseDto, LocationCreateDto, LocationUpdateDto>
    {

    }
}
