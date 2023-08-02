namespace Udemy.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        public CategoriesController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var Categories = _context.Categories.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(Categories);
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult Create()
        {

            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var Category = _mapper.Map<Category>(model);
            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), "only .jpg, .jpeg, .png are allowed!");
                    return PartialView("_Form",model);
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), "maximum picture size is 2MB!");
                    return PartialView("_Form", model);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/categories", imageName);

                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);

                Category.ImageName = imageName;
            }

            
            _context.Categories.Add(Category);
            _context.SaveChanges();

            var viewModel = _mapper.Map<CategoryViewModel>(Category);
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var Category = _context.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CategoryViewModel>(Category);
            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var Category = _context.Categories.Find(model.Id);
            if (Category == null)
            {
                return NotFound();
            }
            else
            {
                Category.Name = model.Name;
            }
            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(Category.ImageName))
                {
                    var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/categories", Category.ImageName);

                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var extension = Path.GetExtension(model.Image.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), "only .jpg, .jpeg, .png are allowed!");
                    return PartialView("_Form", model);
                }

                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), "maximum picture size is 2MB!");
                    return PartialView("_Form", model);
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/categories", imageName);

                using var stream = System.IO.File.Create(path);
                model.Image.CopyTo(stream);

                Category.ImageName = imageName;
            }

            else if (model.Image is null && !string.IsNullOrEmpty(Category.ImageName))
                model.ImageName = Category.ImageName;

            Category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            var viewModel = _mapper.Map<CategoryViewModel>(Category);
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var Category = _context.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }

            Category.IsDeleted = !Category.IsDeleted;
            Category.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return Ok(Category.LastUpdatedOn.ToString());
        }

        public IActionResult AllowItem(CategoryViewModel model)
        {
            var Category = _context.Categories.SingleOrDefault(g => g.Name == model.Name);
            var isAllowed = Category is null || Category.Id == model.Id;

            return Json(isAllowed);
        }

    }
}
