using LearningSystem.Areas.Blog.Models.Articles;
using LearningSystem.Data.Models;
using LearningSystem.Infrastructure.Filters;
using LearningSystem.Infrastructure.Extentions;
using LearningSystem.Services.Blog;
using LearningSystem.Services.Html;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearningSystem.Areas.Blog.Controllers
{
    [Area(WebConstants.BlogArea)]
    [Authorize(Roles = WebConstants.BlogAuthorRole)]
    public class ArticlesController : Controller
    {
        private readonly IHtmlService html;
        private readonly IBlogArticleService articles;
        private readonly UserManager<User> userManager;

        public ArticlesController(IHtmlService html, IBlogArticleService articles, UserManager<User> userManager)
        {
            this.html = html;
            this.articles = articles;
            this.userManager = userManager;
        }

        public IActionResult Create() => View();

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var allArticles = await this.articles.AllAsync(page);

            return View(new ArticleListingViewModel
            {
                Articles = allArticles,
                TotalArticles = await this.articles.TotalAsync(),
                CurrentPage = page
            });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
            => this.ViewOrNotFound(await this.articles.ById(id));

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(PublishArticleFormModel model)
        {
            model.Content = html.Sanitize(model.Content);

            var userId = this.userManager.GetUserId(User);

            await this.articles.CreateAsync(model.Title, model.Content, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
