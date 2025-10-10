namespace ActiviGo.Application.DTOs
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public List<string>? Roles { get; set; }
    }
}
