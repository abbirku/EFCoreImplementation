using Core;
using System;

namespace Infrastructure.Entities
{
    public class StudentRegistration : IEntity<int>
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollDate { get; set; }
        public bool IsPaymentComplete { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
