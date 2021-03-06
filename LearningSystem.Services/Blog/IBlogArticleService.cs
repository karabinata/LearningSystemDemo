﻿using LearningSystem.Services.Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningSystem.Services.Blog
{
    public interface IBlogArticleService
    {
        Task<IEnumerable<BlogArticleListingServiceModel>> AllAsync(int page = 1);

        Task<int> TotalAsync();

        Task<BlogArticleDetailsServiceModel> ById(int id);

        Task CreateAsync(string title, string content, string authorId);
    }
}
