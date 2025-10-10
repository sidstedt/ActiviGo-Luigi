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
    }

}
