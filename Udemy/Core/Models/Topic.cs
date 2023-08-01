namespace Udemy.Core.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Topic:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;
    }
}
