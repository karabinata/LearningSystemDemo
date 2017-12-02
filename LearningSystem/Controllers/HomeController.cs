﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearningSystem.Models;
using LearningSystem.Services;
using System.Threading.Tasks;
using LearningSystem.Models.Home;

namespace LearningSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService courses;
        private readonly IUserService users;

        public HomeController(ICourseService courses, IUserService users)
        {
            this.courses = courses;
            this.users = users;
        }

        public async Task<IActionResult> Index() 
            => View(new HomeIndexViewModel
            {
                Courses = await this.courses.ActiveAsync()
            });

        public async Task<IActionResult> Search(SearchFormModel model)
        {
            var viewModel = new SearchViewModel
            {
                SearchText = model.SearchText
            };

            if (model.SearchInCourses)
            {
                viewModel.Courses = await this.courses.FindAsync(model.SearchText);
            }

            if (model.SearchInUsers)
            {
                viewModel.Users = await this.users.FindAsync(model.SearchText);
            }

            return View(viewModel);
        }

        public IActionResult Error() 
            => View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
    }
}
