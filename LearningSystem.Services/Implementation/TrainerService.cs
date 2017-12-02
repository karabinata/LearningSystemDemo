using System.Threading.Tasks;
using LearningSystem.Services.Models;
using LearningSystem.Data;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Implementation
{
    public class TrainerService : ITrainerService
    {
        private readonly LearningSystemDbContext db;

        public TrainerService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> CoursesAsync(string trainerId)
            => await this.db
                .Courses
                .Where(c => c.TrainerId == trainerId)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();

        public async Task<bool> IsTrainerAsync(int courseId, string trainerId)
            => await this.db
                .Courses
                .AnyAsync(c => c.Id == courseId && c.TrainerId == trainerId);

        public async Task<IEnumerable<StudentInCourseServiceModel>> StudentsInCourseAsync(int courseId)
            => await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.Students.Select(s => s.Student))
                .ProjectTo<StudentInCourseServiceModel>(new { courseId })
                .ToListAsync();

        public async Task<bool> AddGradeAsync(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await this.db.FindAsync<StudentCourse>(studentId, courseId);

            if (studentInCourse == null)
            {
                return false;
            }

            studentInCourse.Grade = grade;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<byte[]> GetExamSubmissionAsync(int courseId, string studentId)
        {
            var studentInCourse = await this.db
                .FindAsync<StudentCourse>(studentId, courseId);

            return studentInCourse?.ExamSubmission;
        }

        public async Task<StudentInCourseNameServiceModel> StudentInCourseNameAsync(int courseId, string studentId)
        {
            var username = await this.db
                .Users
                .Where(u => u.Id == studentId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            if (username == null)
            {
                return null;
            }

            var courseName = await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            if (courseName == null)
            {
                return null;
            }

            return new StudentInCourseNameServiceModel
            {
                Username = username,
                CourseTitle = courseName
            };
        }
    }
}
