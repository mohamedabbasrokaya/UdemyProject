namespace Udemy.Core.Models
{
    public class Category:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        public string? ImagePath { get; set; }
    }
}
