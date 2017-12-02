using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Models.Home
{
    public class SearchFormModel
    {
        public string SearchText { get; set; }

        [Display(Name = "Users")]
        public bool SearchInUsers { get; set; } = true;

        [Display(Name = "Courses")]
        public bool SearchInCourses { get; set; } = true;
    }
}
