namespace Udemy.Controllers
{
    public class CoursesController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        public CoursesController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View("Form", PopulateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", PopulateViewModel(model));
            }

            var course = _mapper.Map<Course>(model);

            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), "only .jpg, .jpeg, .png are allowed!");
                    return View("Form", model);
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), "maximum picture size is 2MB!");
                    return View("Form", model);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/courses", imageName);

                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);

                course.ImageName = imageName;
            }
            _context.Courses.Add(course);
            _context.SaveChanges();     

            return RedirectToAction(nameof(Index));
        }

        private CourseViewModel PopulateViewModel(CourseViewModel? model = null)
        {
            CourseViewModel viewModel = model is null ? new CourseViewModel() : model; 

            var topics = _context.Topics.Where(t => !t.IsDeleted).OrderBy(t => t.Name).AsNoTracking().ToList();
            var categories = _context.Categories.Where(t => !t.IsDeleted).OrderBy(t => t.Name).AsNoTracking().ToList();

            viewModel.Topics = _mapper.Map<IEnumerable<SelectListItem>>(topics);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);

            return viewModel;
        }

        public IActionResult Edit(int id )
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();
            var viewModel = _mapper.Map<CourseViewModel>(course);

            return View("Form", PopulateViewModel(viewModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", PopulateViewModel(model));
            }


            var course = _context.Courses.Find(model.Id);
            if (course == null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(course.ImageName))
                {
                    var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/courses", course.ImageName);
                    if(System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), "only .jpg, .jpeg, .png are allowed!");
                    return View("Form", model);
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), "maximum picture size is 2MB!");
                    return View("Form", model);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/courses", imageName);

                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);

                model.ImageName = imageName;
            }else if(model.Image is null && !string.IsNullOrEmpty(course.ImageName)) {
                model.ImageName=course.ImageName;
            }
            course = _mapper.Map(model,course);
            course.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
