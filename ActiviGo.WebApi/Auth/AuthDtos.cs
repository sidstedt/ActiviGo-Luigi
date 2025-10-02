namespace ActiviGo.WebApi.Auth
{
    public record RegisterDto(string Email, string Password, string FirstName, string LastName);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string AccessToken);
}
