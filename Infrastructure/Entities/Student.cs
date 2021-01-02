using Core;
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Student : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<StudentRegistration> Courses { get; set; }
    }
}
