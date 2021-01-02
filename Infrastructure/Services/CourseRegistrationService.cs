using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ICourseRegistrationService : IDisposable
    {
        Task<ValidationModel> RegisterStudentAsync(RegistrationInfo registration);
        Task<ValidationModel> UpdateRegisteration(RegistrationInfo registration);
        Task<ValidationModel> DeletRegisteration(int id);
    }

    public class CourseRegistrationService : ICourseRegistrationService
    {

        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public CourseRegistrationService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public async Task<ValidationModel> DeletRegisteration(int id)
        {
            try
            {
                if(id == 0)
                    return new ValidationModel { IsValid = false, Message = "Please provide a valid Id" };

                var registration = _courseUnitOfWork.StudentRegistrationRepository.GetById(id);
                var student = _courseUnitOfWork.StudentRepository.GetById(registration.StudentId);
                var course = _courseUnitOfWork.CourseRepository.GetById(registration.CourseId);

                course.SeatCount += 1;
                _courseUnitOfWork.StudentRegistrationRepository.Remove(registration);
                _courseUnitOfWork.CourseRepository.Edit(course);

                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{student.Name} has been successfully removed from {course.Title} course." };
            }
            catch (Exception ex)
            {
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel> RegisterStudentAsync(RegistrationInfo registration)
        {
            try
            {
                var validation = registration.IsValid();
                if (!validation.IsValid)
                    return validation;

                var student = _courseUnitOfWork.StudentRepository.GetById(registration.StudentRegistration.StudentId);
                var course = _courseUnitOfWork.CourseRepository.GetById(registration.StudentRegistration.CourseId);

                if (course.SeatCount == 0)
                    return new ValidationModel { IsValid = true, Message = "House full for this course" };

                course.SeatCount -= 1;
                _courseUnitOfWork.StudentRegistrationRepository.Add(registration.StudentRegistration);
                _courseUnitOfWork.CourseRepository.Edit(course);

                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{student.Name} has been successfully registered with {course.Title} course." };
            }
            catch (Exception ex)
            {
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel> UpdateRegisteration(RegistrationInfo registration)
        {
            try
            {
                var validation = registration.IsValid();
                if (!validation.IsValid)
                    return validation;

                var student = _courseUnitOfWork.StudentRepository.GetById(registration.StudentRegistration.StudentId);
                var course = _courseUnitOfWork.CourseRepository.GetById(registration.StudentRegistration.CourseId);

                _courseUnitOfWork.StudentRegistrationRepository.Edit(registration.StudentRegistration);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{student.Name} registration has been updated to {course.Title} course." };
            }
            catch (Exception ex)
            {
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public void Dispose()
        {
            _courseUnitOfWork.Dispose();
        }
    }
}
