namespace Udemy.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Topic, TopicViewModel>().ReverseMap();
            CreateMap<Topic, SelectListItem>()
                .ForMember(dest => dest.Value,opt =>opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text,opt =>opt.MapFrom(src => src.Name));

            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Course, CourseViewModel>().ReverseMap();  
        }
    }
}
