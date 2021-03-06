﻿using LearningSystem.Services.Models;
using System.Collections.Generic;

namespace LearningSystem.Models.Trainer
{
    public class StudentsInCourseViewModel
    {
        public IEnumerable<StudentInCourseServiceModel> Students { get; set; }

        public CourseListingServiceModel Course { get; set; }
    }
}
