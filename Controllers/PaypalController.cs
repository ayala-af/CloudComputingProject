using Microsoft.AspNetCore.Mvc;

namespace CloudComputingProject.Controllers
{
    public class Paypal : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
