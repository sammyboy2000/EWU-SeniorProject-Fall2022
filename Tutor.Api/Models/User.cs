﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Tutor.Api.Models
{
    public partial class User
    {
        public User()
        {
            Admins = new HashSet<Admin>();
            Students = new HashSet<Student>();
            Tutors = new HashSet<Tutor>();
        }

        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public bool IsStudent { get; set; }
        public bool IsTutor { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Tutor> Tutors { get; set; }
    }
}