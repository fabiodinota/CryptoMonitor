using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers
{
    public class UserReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
