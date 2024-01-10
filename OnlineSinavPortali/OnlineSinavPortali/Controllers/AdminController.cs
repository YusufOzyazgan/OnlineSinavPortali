using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace OnlineSinavPortali.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddTest()
        {
            return View();
        }
        public IActionResult EditTest()
        {
            return View();
        }
    }
}   
