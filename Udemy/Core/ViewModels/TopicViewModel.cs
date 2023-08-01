namespace Udemy.Core.ViewModels
{
    public class TopicViewModel:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100,ErrorMessage ="The name can't be more than 100char")]
        [Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = "Topic with the same name already exists!")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "The Description can't be more than 500char")]
        public string Description { get; set; } = null!;
    }
}
