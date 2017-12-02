using System.Threading.Tasks;
using LearningSystem.Services.Models;
using LearningSystem.Data;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LearningSystem.Data.Models;
using System;

namespace LearningSystem.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly LearningSystemDbContext db;
        private readonly IPdfGenerator pdfGenerator;

        public UserService(LearningSystemDbContext db, IPdfGenerator pdfGenerator)
        {
            this.db = db;
            this.pdfGenerator = pdfGenerator;
        }

        public async Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return await this.db
                .Users
                .OrderBy(u => u.UserName)
                .Where(u => u.UserName.ToLower().Contains(searchText.ToLower()))
                .ProjectTo<UserListingServiceModel>()
                .ToListAsync();
        }

        public async Task<byte[]> GetPdfCertificate(int courseId, string studentId)
        {
            var studentInCourse = await this.db
                .FindAsync<StudentCourse>(studentId, courseId);

            if (studentInCourse == null)
            {
                return null;
            }

            var data = await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new
                {
                    CourseName = c.Name,
                    CourseStartDate = c.StartDate,
                    CourseEndDate = c.EndDate,
                    StudentName = c.Students
                          .Where(s => s.StudentId == studentId)
                          .Select(s => s.Student.Name)
                          .FirstOrDefault(),
                    StudentGrade = c.Students
                          .Where(s => s.StudentId == studentId)
                          .Select(s => s.Grade)
                          .FirstOrDefault(),
                    TrainerName = c.Trainer.Name
                })
                .FirstOrDefaultAsync();

            return this.pdfGenerator.GeneratePdfFromHtml(
                string.Format(ServiceConstants.PdfCertificateFormat, 
                data.CourseName, 
                data.CourseStartDate.ToString("dd/MM/yyyy"), 
                data.CourseEndDate.ToString("dd/MM/yyyy"),
                data.StudentName,
                data.StudentGrade,
                data.TrainerName,
                DateTime.UtcNow.ToString("dd/MM/yyyy")));
        }

        public async Task<UserProfileServiceModel> ProfileAsync(string id)
            => await this.db
                .Users
                .Where(u => u.Id == id)
                .ProjectTo<UserProfileServiceModel>(new { studentId = id })
                .FirstOrDefaultAsync();
    }
}
