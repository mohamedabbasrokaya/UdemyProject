using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Udemy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var Categories = _context.Categories.AsNoTracking().OrderByDescending(c=>c.CreatedOn).Take(8).ToList();
            var categoriesViewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(Categories);

            var Topics = _context.Topics.AsNoTracking().OrderByDescending(c => c.CreatedOn).Take(4).ToList();
            var topicsviewModel = _mapper.Map<IEnumerable<TopicViewModel>>(Topics);

            ViewBag.topicsviewModel = topicsviewModel;
            return View(categoriesViewModel);     
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}