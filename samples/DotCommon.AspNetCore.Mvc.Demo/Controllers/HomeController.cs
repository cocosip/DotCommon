using Microsoft.AspNetCore.Mvc;

namespace DotCommon.AspNetCore.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return Json(new { id = 1, name = "haha" });
        }
    }
}
