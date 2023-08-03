namespace Udemy.Core.ViewModels
{
    public class CourseViewModel:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(200,ErrorMessage ="Title can't be more than 200char")]
        public string Title { get; set; } = null!;

        [MaxLength(800, ErrorMessage = "Description can't be more than 800char")]
        public string Description { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Instructor name can't be more than 100char")]
        public string Instructor { get; set; } = null!;
        public double Rating { get; set; }
        public double? OldPrice { get; set; }
        public double ActualPrice { get; set; }

        [Display(Name = "Is This Course Best Seller?")]
        public bool BestSeller { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }

        [Display(Name ="Topic")]
        public int TopicId { get; set; }
        public IEnumerable<SelectListItem>? Topics { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }


    }
}
