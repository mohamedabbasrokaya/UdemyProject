namespace Udemy.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Topic, TopicViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
        }
    }
}
