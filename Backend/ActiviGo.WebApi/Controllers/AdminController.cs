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
                return Forbid("Administratörsrollen måste tilldelas manuellt eller via en dedikerad Admin-endpoint.");
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

        // PUT: api/users/{userId}
        [HttpPut("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Uppdatera SecurityStamp om saknas
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _userManager.UpdateAsync(user);
            }

            // Uppdatera fält
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Email ?? user.UserName;
            user.IsActive = request.IsActive ?? user.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(new { message = "Failed to update user", errors = updateResult.Errors });

            // Om roller ska uppdateras
            if (request.Roles != null && request.Roles.Any())
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return BadRequest(new { message = "Failed to remove old roles" });

                var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!addResult.Succeeded)
                    return BadRequest(new { message = "Failed to assign new roles" });
            }

            var updatedRoles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.IsActive,
                Roles = updatedRoles,
                message = "User updated successfully"
            });
        }

        // DELETE: api/users/{userId}
        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound(new { message = "User not found" });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to delete user", errors = result.Errors });

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
