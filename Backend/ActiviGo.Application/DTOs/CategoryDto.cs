namespace ActiviGo.Application.DTOs
{
    
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int? ActivityId { get; set; }
        public string? ActivityName { get; set; }
    }

    // Detailed version with activites 
    public class CategoryWithActivitiesDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<ActivityForCategoryDto> Activities { get; set; } = new();
    }

    public class CategoryUpdateDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
