﻿using BrainBenchmarkAPI.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BrainBenchmarkAPI.Models
{
    public class UserModel
    {
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [AllowNull]
        public DateTime? Birthday { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public DateTime CreateTime { get; set; }

        public UserModel(UserDb user)
        {
            Name = user.Name;
            Email = user.Email;
            Birthday = user.Birthdate;
            Gender = user.Gender;
            CreateTime = user.CreateTime;
        }
    }
}
