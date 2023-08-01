namespace Udemy.Core.ViewModels
{
    public class CategoryViewModel:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100,ErrorMessage = "The name can't be more than 100char")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "The Description can't be more than 500char")]
        public string Description { get; set; } = null!;

        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
    }
}
