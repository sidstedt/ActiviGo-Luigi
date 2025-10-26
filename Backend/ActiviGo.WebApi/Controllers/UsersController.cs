using ActiviGo.Application.DTOs;
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
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<User> userManager, IEmailService emailService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
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
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            try
            {
                if (request is null)
                    return BadRequest(new { message = "Invalid payload." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(request.CurrentPassword) ||
                    string.IsNullOrWhiteSpace(request.NewPassword) ||
                    string.IsNullOrWhiteSpace(request.ConfirmPassword))
                {
                    return BadRequest(new { message = "Alla fält måste fyllas i." });
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return BadRequest(new { message = "De nya lösenorden matchar inte." });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized(new { message = "Kunde inte hitta användaren." });

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        message = "Byte av lösenord misslyckades.",
                        errors = result.Errors
                    });
                }

                // INTE nödvändig i JWT-scenario (kan kasta)
                // await _signInManager.RefreshSignInAsync(user);

                return Ok(new { message = "Lösenordet är uppdaterat." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword threw.");
                return StatusCode(500, new
                {
                    message = "Internal error in ChangePassword.",
                    detail = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }


        }
}
