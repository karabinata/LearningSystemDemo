using System.Collections.Generic;
using System.Threading.Tasks;
using LearningSystem.Services.Models;
using LearningSystem.Data;
using System.Linq;
using System;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext db;

        public CourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> ActiveAsync()
            => await this.db
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.StartDate >= DateTime.UtcNow)
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();

        public async Task<IEnumerable<CourseListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return await this.db
                .Courses
                .OrderByDescending(c => c.Id)
                .Where(c => c.Name.ToLower().Contains(searchText.ToLower()))
                .ProjectTo<CourseListingServiceModel>()
                .ToListAsync();
        }

        public async Task<TModel> ByIdAsync<TModel>(int id) where TModel : class
            => await this.db
                .Courses
                .Where(c => c.Id == id)
                .ProjectTo<TModel>()
                .FirstOrDefaultAsync();
        
        public async Task<bool> SignUpStudentAsync(int courseId, string studentId)
        {
            var courseInfo = this.GetCourseInfo(courseId, studentId);

            if (
                courseInfo == null
                || courseInfo.Result.StartDate < DateTime.UtcNow
                || courseInfo.Result.UserIsEnrolledInCourse)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = studentId
            };

            this.db.Add(studentInCourse);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SignOutStudentAsync(int courseId, string studentId)
        {
            var courseInfo = this.GetCourseInfo(courseId, studentId);

            if (
                courseInfo == null
                || courseInfo.Result.StartDate < DateTime.UtcNow
                || !courseInfo.Result.UserIsEnrolledInCourse)
            {
                return false;
            }

            var studentInCourse = await this.db.FindAsync<StudentCourse>(studentId, courseId);

            this.db.Remove(studentInCourse);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StudentIsEnrolledInCourseAsync(int courseId, string userId)
            => await this.db
                    .Courses
                    .AnyAsync(c => c.Id == courseId && c.Students.Any(s => s.StudentId == userId));

        private async Task<CourseWithStudentsInfo> GetCourseInfo(int courseId, string studentId)
            => await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseWithStudentsInfo
                {
                    StartDate = c.StartDate,
                    UserIsEnrolledInCourse = c.Students.Any(s => s.StudentId == studentId)
                })
                .FirstOrDefaultAsync();

        public async Task<bool> SaveExamSubmission(int courseId, string studentId, byte[] examSubmission)
        {
            var studentInCouse = await this.db
                .FindAsync<StudentCourse>(studentId, courseId);

            if (studentInCouse == null)
            {
                return false;
            }

            studentInCouse.ExamSubmission = examSubmission;

            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
