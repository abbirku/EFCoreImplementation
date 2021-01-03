using Autofac;
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
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly ICourseRegistrationService _courseRegistrationService;

        public RegistrationModel()
        {
            _studentService = Startup.AutofacContainer.Resolve<IStudentService>();
            _courseService = Startup.AutofacContainer.Resolve<ICourseService>();
            _courseRegistrationService = Startup.AutofacContainer.Resolve<ICourseRegistrationService>();
        }

        internal RegistrationViewModel<StudentRegistration> CreateStudentViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedRegistration = new StudentRegistration { CourseId = 0, StudentId = 0, EnrollDate = DateTime.Now };
            if (id != 0)
            {
                var registrationValidationModel = _courseRegistrationService.GetStudentRegistrationById(id);
                if (!registrationValidationModel.IsValid)
                    return new RegistrationViewModel<StudentRegistration> { Validation = registrationValidationModel };

                selectedRegistration = registrationValidationModel.Data;
            }

            var model = new RegistrationViewModel<StudentRegistration>
            {
                Registrations = _courseRegistrationService.GetRegistrations(),
                Courses = _courseService.GetCourses(),
                Students = _studentService.GetStudents(),
                SelectedRegistration = selectedRegistration,
                Validation = new ValidationModel<StudentRegistration> { IsValid = isValid, Data = selectedRegistration, Message = message }
            };

            return model;
        }

        internal async Task<ValidationModel<StudentRegistration>> RegisterStudentAsync(RegistrationInfo registration)
        {
            return await _courseRegistrationService.RegisterStudentAsync(registration);
        }

        internal async Task<ValidationModel<StudentRegistration>> UpdateRegisteration(RegistrationInfo registration)
        {
            return await _courseRegistrationService.UpdateRegisteration(registration);
        }

        internal async Task<ValidationModel<StudentRegistration>> DeletRegisteration(int id)
        {
            return await _courseRegistrationService.DeletRegisteration(id);
        }

        public void Dispose()
        {
            _courseRegistrationService.Dispose();
        }
    }

    public class RegistrationViewModel<T>
    {
        public StudentRegistration SelectedRegistration { get; set; }
        public IList<Course> Courses { get; set; }
        public IList<StudentDTO> Students { get; set; }
        public IList<RegistrationDTO> Registrations { get; set; }
        public ValidationModel<T> Validation { get; set; }
    }
}
