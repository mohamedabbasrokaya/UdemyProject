namespace Udemy.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TopicsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public  IActionResult Index()
        {
            var Topics = _context.Topics.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<TopicViewModel>>(Topics);
            return View(viewModel);
        }

        [AjaxOnly]
        public IActionResult Create()
        {

            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TopicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var Topic = _mapper.Map<Topic>(model);
            _context.Topics.Add(Topic);
            _context.SaveChanges();

            var viewModel = _mapper.Map<TopicViewModel>(Topic);
            return PartialView("_TopicRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var Topic = _context.Topics.Find(id);
            if (Topic == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<TopicViewModel>(Topic);
            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TopicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var Topic = _context.Topics.Find(model.Id);
            if (Topic == null)
            {
                return NotFound();
            }
            else
            {
                Topic.Name = model.Name;
            }
            Topic.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();

            var viewModel = _mapper.Map<TopicViewModel>(Topic);
            return PartialView("_TopicRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var Topic = _context.Topics.Find(id);
            if (Topic == null)
            {
                return NotFound();
            }

            Topic.IsDeleted = !Topic.IsDeleted;
            Topic.LastUpdatedOn = DateTime.Now;
            _context.SaveChanges();
            return Ok(Topic.LastUpdatedOn.ToString());
        }

        public IActionResult AllowItem(TopicViewModel model)
        {
            var Topic = _context.Topics.SingleOrDefault(g => g.Name == model.Name);
            var isAllowed = Topic is null || Topic.Id == model.Id;

            return Json(isAllowed);
        }

    }
}
