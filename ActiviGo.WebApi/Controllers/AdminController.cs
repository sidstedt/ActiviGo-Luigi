using ActiviGo.Application.DTOs;
using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IActivityService _activityService;
        private readonly IActivityOccurrenceService _activityOccurrenceService;

        private readonly IUnitofWork _uow;

        public AdminController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IActivityService activityService,
            IActivityOccurrenceService activityOccurrenceService,
            IUnitofWork uow)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _activityService = activityService;
            _activityOccurrenceService = activityOccurrenceService;
            _uow = uow;
        }

        [HttpPost("activities")]
        public async Task<ActionResult<ActivityResponseDto>> CreateActivity([FromBody] ActivityCreateDto createDto)
        {
            var activity = await _activityService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
        }

        [HttpDelete("activities/{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var result = await _activityService.DeleteAsync(id);
            if (!result)
            {
                return NotFound($"Activity med ID {id} hittades inte.");
            }
            return NoContent();
        }

        [HttpGet("activities/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ActivityResponseDto>> GetActivity(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] string? role = null)
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Om en roll har specificerats, filtrera bort användare som inte har den rollen
                if (!string.IsNullOrEmpty(role) && !roles.Contains(role, StringComparer.OrdinalIgnoreCase))
                    continue;

                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    Roles = roles
                });
            }

            return Ok(userList);
        }


        [HttpPost("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> AddRoleToUser(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound($"Användare hittades inte.");
            }

            if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("Administratörsrollen måste tilldelas manuellt eller via én dedikerad Admin-endpoint.");
            }
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest($"Rollen '{roleName}' existerar inte.");
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Rollen '{roleName}' tilldelades användare '{user.Email}'.");
            }
            return BadRequest(result.Errors);
        }

        [HttpPut("users/{userId}/roles")]
        public async Task<ActionResult> UpdateUserRoles(string userId, [FromBody] List<string> roles)
        {
            // 1Hitta användaren
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Ingen användare med ID {userId} hittades.");

            // Hantera säkerhetsstämpel (om nödvändigt)
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }

            // Hämta nuvarande roller
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Ta bort roller som inte längre ska finnas
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return BadRequest("Misslyckades med att ta bort befintliga roller.");

            // Lägg till de nya rollerna
            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
                return BadRequest("Misslyckades med att lägga till nya roller.");

            // Returnera uppdaterad information
            var updatedRoles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                Roles = updatedRoles
            });
        }

        [HttpPost("zones")]
        public async Task<IActionResult> CreateNewZone([FromBody] Zone newZone)
        {
            await _uow.Zone.AddAsync(newZone);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetZone", new { id = newZone.Id }, newZone);
        }
    }
}
