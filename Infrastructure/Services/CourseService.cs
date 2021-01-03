using Infrastructure.BusinessObject;
using Infrastructure.Entities;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ICourseService : IDisposable
    {
        ValidationModel<Course> GetCourseById(int id);
        IList<Course> GetCourses();
        Task<ValidationModel<Course>> Createcourse(CourseInfo courseInfo);
        Task<ValidationModel<Course>> UpdateCourseInfo(CourseInfo courseInfo);
        Task<ValidationModel<Course>> RemoveCourse(int id);

    }

    public class CourseService : ICourseService
    {
        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public CourseService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public ValidationModel<Course> GetCourseById(int id)
        {
            try
            {
                var course = _courseUnitOfWork.CourseRepository.GetById(id);
                if (course == null)
                    throw new Exception($"Course does not exists with {id} Id");

                return new ValidationModel<Course> { IsValid = true, Data = course, Message = "Data found" };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Course> { IsValid = false, Message = ex.Message };
            }
        }

        public IList<Course> GetCourses()
        {
            return _courseUnitOfWork.CourseRepository.GetCourses();
        }

        public async Task<ValidationModel<Course>> Createcourse(CourseInfo courseInfo)
        {
            try
            {
                var validation = courseInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel<Course> { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.CourseRepository.Add(courseInfo.Course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel<Course> { IsValid = true, Data = courseInfo.Course, Message = $"{courseInfo.Course.Title} has been successfully created." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Course> { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel<Course>> UpdateCourseInfo(CourseInfo courseInfo)
        {
            try
            {
                var validation = courseInfo.IsValid();
                if (!validation.IsValid)
                    throw new Exception(validation.Message);

                _courseUnitOfWork.CourseRepository.Edit(courseInfo.Course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel<Course> { IsValid = true, Data = courseInfo.Course, Message = $"{courseInfo.Course.Title} has been successfully updated." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Course> { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel<Course>> RemoveCourse(int id)
        {
            try
            {
                if (id == 0)
                    return new ValidationModel<Course> { IsValid = false, Message = "Please provide a valid Id" };

                var course = _courseUnitOfWork.CourseRepository.GetById(id);
                if (course == null)
                    throw new Exception("Course does not exists.");

                _courseUnitOfWork.CourseRepository.Remove(course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel<Course> { IsValid = true, Data = course, Message = $"{course.Title} has been successfully remove." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Course> { IsValid = false, Message = ex.Message };
            }
        }

        public void Dispose()
        {
            _courseUnitOfWork.Dispose();
        }
    }
}
