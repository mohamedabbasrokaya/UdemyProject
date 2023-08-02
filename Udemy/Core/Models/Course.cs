namespace Udemy.Core.Models
{
    public class Course:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(800)]
        public string Description { get; set; } = null!;

        [MaxLength(100)]
        public string Instructor { get; set; } = null!;
        public double Rating { get; set; }
        public double? OldPrice { get; set; }
        public double ActualPrice { get; set; }
        public bool BestSeller { get; set; }
        public string? ImageName { get; set; }

        public int TopicId { get; set; }
        public Topic? Topic { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
