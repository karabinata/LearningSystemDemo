﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataConstants.UserNameMinLength)]
        [MaxLength(DataConstants.UserNameMaxLength)]
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public List<Course> Trainings { get; set; } = new List<Course>();

        public List<StudentCourse> Courses { get; set; } = new List<StudentCourse>();

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
