using LearningSystem.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Areas.Blog.Models.Articles
{
    public class PublishArticleFormModel
    {
        [Required]
        [MinLength(DataConstants.ArticleTitleMinLength)]
        [MaxLength(DataConstants.ArticleTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
