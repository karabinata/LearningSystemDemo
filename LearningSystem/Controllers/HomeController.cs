using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearningSystem.Models;
using LearningSystem.Services;
using System.Threading.Tasks;

namespace LearningSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService courses;

        public HomeController(ICourseService courses)
        {
            this.courses = courses;
        }

        public async Task<IActionResult> Index() => View(await this.courses.ActiveAsync());

        public IActionResult Error() 
            => View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
    }
}
