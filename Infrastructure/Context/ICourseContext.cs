using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Context
{
    public interface ICourseContext
    {
        DbSet<Student> Students { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<StudentRegistration> StudentRegistrations { get; set; }
    }
}
