using LearningSystem.Data;
using LearningSystem.Data.Models;
using LearningSystem.Infrastructure.Extentions;
using LearningSystem.Models.Courses;
using LearningSystem.Services;
using LearningSystem.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LearningSystem.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courses;
        private readonly UserManager<User> userManager;

        public CoursesController(ICourseService courses, UserManager<User> userManager)
        {
            this.courses = courses;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new CourseDetailsViewModel
            {
                Course = await this.courses.ByIdAsync<CourseDetailsServiceModel>(id),
            };

            if (model.Course == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(User);
                model.UserIsEnrolledInCourse = await this.courses.StudentIsEnrolledInCourseAsync(id, userId);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitExam(int id, IFormFile exam)
        {
            if (!exam.FileName.EndsWith(".zip") 
                || exam.Length > DataConstants.CourseExamSubmissionFileLength)
            {
                TempData.AddErrorMessage("Your submission should a '.zip' file with no more tha 2 MB in size!");

                return RedirectToAction(nameof(Details), new { id });
            }

            var fileContents = await exam.ToByteArrayAsync();
            var userId = this.userManager.GetUserId(User);

            var success = await this.courses.SaveExamSubmission(id, userId, fileContents);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Exam submission saved successfully!");
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUp(int id)
        {
            var studentId = this.userManager.GetUserId(User);

            var success = await this.courses.SignUpStudentAsync(id, studentId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Thank you for your registration in the course!");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut(int id)
        {
            var studentId = this.userManager.GetUserId(User);

            var success = await this.courses.SignOutStudentAsync(id, studentId);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccessMessage("Sorry to see you leaving.");

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
