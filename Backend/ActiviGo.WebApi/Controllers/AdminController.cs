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

        public AdminController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IActivityService activityService,
            IActivityOccurrenceService activityOccurrenceService
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _activityService = activityService;
            _activityOccurrenceService = activityOccurrenceService;
        }

        // -------------------------------------------------------------------
        // Hämta alla användare (valfritt filtrerat på roll)
        // -------------------------------------------------------------------
        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] string? role = null)
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList();

                if (!string.IsNullOrEmpty(role) && !roles.Contains(role, StringComparer.OrdinalIgnoreCase))
                    continue;

                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.IsActive,
                    user.CreatedAt,
                    Roles = roles
                });
            }

            return Ok(userList);
        }

        // -------------------------------------------------------------------
        // Lägg till roll för användare
        // -------------------------------------------------------------------
        [HttpPost("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> AddRoleToUser(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound("Användare hittades inte.");

            if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return Forbid("Administratörsrollen måste tilldelas manuellt eller via en dedikerad Admin-endpoint.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest($"Rollen '{roleName}' existerar inte.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return Ok($"Rollen '{roleName}' tilldelades användare '{user.Email}'.");

            return BadRequest(result.Errors);
        }

        // -------------------------------------------------------------------
        // Uppdatera användare (ex. namn, e-post, aktiv status)
        // -------------------------------------------------------------------
        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound("Användare hittades inte.");

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Email ?? user.UserName;
            user.IsActive = request.IsActive ?? user.IsActive;

            // Se till att SecurityStamp inte är null
            if (string.IsNullOrEmpty(user.SecurityStamp))
                user.SecurityStamp = Guid.NewGuid().ToString();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Uppdatera roller (om skickade)
            if (request.Roles != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var rolesToRemove = currentRoles.Except(request.Roles, StringComparer.OrdinalIgnoreCase);
                if (rolesToRemove.Any())
                    await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                var rolesToAdd = request.Roles.Except(currentRoles, StringComparer.OrdinalIgnoreCase);
                foreach (var role in rolesToAdd)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                        await _userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok($"Användaren '{user.Email}' har uppdaterats.");
        }

        // -------------------------------------------------------------------
        // Ta bort användare
        // -------------------------------------------------------------------
        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound("Användare hittades inte.");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Forbid("Du kan inte ta bort en administratör.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok($"Användaren '{user.Email}' har tagits bort.");

            return BadRequest(result.Errors);
        }
    }
}
