using LearningSystem.Data.Models;
using LearningSystem.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningSystem.Services
{
    public interface ITrainerService
    {
        Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string trainerId);

        Task<bool> IsTrainerAsync(int courseId, string trainerId);

        Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId);

        Task<bool> AddGradeAsync(int courseId, string studentId, Grade grade);

        Task<byte[]> GetExamSubmissionAsync(int courseId, string studentId);

        Task<StudentInCourseNameServiceModel> StudentInCourseNameAsync(int courseId, string studentId);
    }
}
