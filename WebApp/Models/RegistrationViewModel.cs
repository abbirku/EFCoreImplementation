using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Context;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class RegistrationModel : IDisposable
    {
        private readonly ICourseUnitOfWork _courseUnitOfWork;
        private readonly ICourseRegistrationService _courseRegistrationService;

        public RegistrationModel(ICourseUnitOfWork courseUnitOfWork, ICourseRegistrationService courseRegistrationService)
        {
            _courseUnitOfWork = courseUnitOfWork;
            _courseRegistrationService = courseRegistrationService;
        }

        public RegistrationViewModel CreateStudentViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedRegistration = new StudentRegistration { CourseId = 0, StudentId = 0, EnrollDate = DateTime.Now };
            if (id != 0)
            {
                selectedRegistration = _courseUnitOfWork.StudentRegistrationRepository.GetById(id);
            }

            var model = new RegistrationViewModel
            {
                Registrations = _courseUnitOfWork.StudentRegistrationRepository.GetRegistrations(),
                Courses = _courseUnitOfWork.CourseRepository.GetAll().ToList(),
                Students = _courseUnitOfWork.StudentRepository.GetAll().ToList(),
                SelectedRegistration = selectedRegistration,
                Validation = new ValidationModel { IsValid = isValid, Message = message }
            };

            return model;
        }

        public void Dispose()
        {
            _courseRegistrationService.Dispose();
        }
    }

    public class RegistrationViewModel
    {
        public StudentRegistration SelectedRegistration { get; set; }
        public List<Course> Courses { get; set; }
        public List<Student> Students { get; set; }
        public List<RegistrationDTO> Registrations { get; set; }
        public ValidationModel Validation { get; set; }
    }
}
