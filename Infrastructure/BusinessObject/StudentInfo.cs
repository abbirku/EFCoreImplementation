using Infrastructure.Entities;
using System;

namespace Infrastructure.BusinessObject
{
    public class StudentInfo
    {
        public Student Student { get; set; }

        public ValidationModel IsValid()
        {
            if (Student == null)
                return new ValidationModel { IsValid = false, Message = "No student has been provided" };

            if (string.IsNullOrEmpty(Student.Name))
                return new ValidationModel { IsValid = false, Message = "Student name can not be null or empty" };

            if (Student.DateOfBirth == null)
                return new ValidationModel { IsValid = false, Message = "Date of birth can not be null" };

            var age = DateTime.Now.Year - Student.DateOfBirth.Year;

            if(age < 18)
                return new ValidationModel { IsValid = false, Message = "Teenager student is not allowed for this course" };

            return new ValidationModel { IsValid = true, Message = "Student information is valid" };
        }
    }
}
