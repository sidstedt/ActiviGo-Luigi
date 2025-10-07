namespace ActiviGo.Application.DTOs.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
