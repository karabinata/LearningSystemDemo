using Ganss.XSS;

namespace LearningSystem.Services.Html.Implementations
{
    public class HtmlService : IHtmlService
    {
        private readonly HtmlSanitizer sanitizer;

        public HtmlService()
        {
            this.sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string htmlContent)
            => this.sanitizer.Sanitize(htmlContent);
    }
}
