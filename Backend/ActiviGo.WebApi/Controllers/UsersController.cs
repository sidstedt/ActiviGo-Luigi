using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActiviGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public UsersController(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        [Authorize(Roles =("Admin,Staff"))]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("staff")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");

            var result = staffUsers.Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{userId:guid}")]
        [Authorize(Roles = ("Admin,Staff"))]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] Application.DTOs.ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Ok(new { message = "If the email exists, a reset link has been sent." });

            // Skapa token och länk
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var resetLink = $"{request.ResetUrl}?email={request.Email}&token={encodedToken}";

            // Läs in HTML-mallen
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PasswordResetTemplate.html");
            if (!System.IO.File.Exists(templatePath))
                return StatusCode(500, new { message = "Email template not found." });

            var htmlTemplate = await System.IO.File.ReadAllTextAsync(templatePath);

            // Ersätt platshållare i mallen
            var body = htmlTemplate
                .Replace("{{FirstName}}", user.FirstName ?? "användare")
                .Replace("{{ResetLink}}", resetLink)
                .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

            // Skicka mejlet
            var subject = "Återställ ditt lösenord – ActiviGo";
            await _emailService.SendEmailAsync(user.Email, subject, body);

            return Ok(new { message = "Password reset link sent to email." });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] Application.DTOs.ResetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid request." });
            }

            var decodedToken = Uri.UnescapeDataString(request.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Password reset failed.", errors = result.Errors });
            }

            return Ok(new { message = "Password has been reset successfully." });
        }
    }
}
