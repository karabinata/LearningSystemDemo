using LearningSystem.Services.Models;

namespace LearningSystem.Models.Courses
{
    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel Course { get; set; }

        public bool UserIsEnrolledInCourse { get; set; }
    }
}
