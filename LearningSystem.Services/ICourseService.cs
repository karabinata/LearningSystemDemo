using LearningSystem.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningSystem.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> ActiveAsync();

        Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText);

        Task<TModel> ByIdAsync<TModel>(int id) where TModel : class;

        Task<bool> SignUpStudentAsync(int courseId, string studentId);

        Task<bool> SignOutStudentAsync(int id, string studentId);

        Task<bool> StudentIsEnrolledInCourseAsync(int courseId, string userId);

        Task<bool> SaveExamSubmission(int courseId, string studentId, byte[] examSubmission);
    }
}
