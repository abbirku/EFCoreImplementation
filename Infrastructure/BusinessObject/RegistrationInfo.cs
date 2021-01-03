using Core;
using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BusinessObject
{
    public class RegistrationInfo
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;

        public RegistrationInfo(IStudentRepository studentRepository,
            ICourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public StudentRegistration StudentRegistration { get; set; }

        public ValidationModel<StudentRegistration> IsValid()
        {
            if(StudentRegistration.StudentId == 0)
                return new ValidationModel<StudentRegistration> { IsValid = false, Message = "Please select a student" };

            var student = _studentRepository.GetById(StudentRegistration.StudentId);
            if (student == null)
                return new ValidationModel<StudentRegistration> { IsValid = false, Message = "Selected student does not exists" };

            if (StudentRegistration.CourseId == 0)
                return new ValidationModel<StudentRegistration> { IsValid = false, Message = "Please select a course" };

            var course = _courseRepository.GetById(StudentRegistration.CourseId);
            if (course == null)
                return new ValidationModel<StudentRegistration> { IsValid = false, Message = "Selected course does not exists" };

            //Checking is registration date is earlier then 2020
            var result = DateTime.Compare(StudentRegistration.EnrollDate, DateTime.Now);
            if(result < 0 && StudentRegistration.EnrollDate.Year <= 2019) 
                return new ValidationModel<StudentRegistration> { IsValid = false, Message = "Registration year must be after 2019" };

            return new ValidationModel<StudentRegistration> { IsValid = true, Data = StudentRegistration, Message = "Registration information is valid" };
        }


    }
}
