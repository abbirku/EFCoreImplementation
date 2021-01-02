using Infrastructure.BusinessObject;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ICourseService : IDisposable
    {
        Task<ValidationModel> Createcourse(CourseInfo courseInfo);
        Task<ValidationModel> UpdateCourseInfo(CourseInfo courseInfo);
        Task<ValidationModel> RemoveCourse(int id);

    }

    public class CourseService : ICourseService
    {
        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public CourseService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public async Task<ValidationModel> Createcourse(CourseInfo courseInfo)
        {
            try
            {
                var validation = courseInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.CourseRepository.Add(courseInfo.Course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{courseInfo.Course.Title} has been successfully created." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                throw new Exception(ex.Message);
            }
        }

        public async Task<ValidationModel> UpdateCourseInfo(CourseInfo courseInfo)
        {
            try
            {
                var validation = courseInfo.IsValid();
                if (!validation.IsValid)
                    throw new Exception(validation.Message);

                _courseUnitOfWork.CourseRepository.Edit(courseInfo.Course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{courseInfo.Course.Title} has been successfully updated." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                throw new Exception(ex.Message);
            }
        }

        public async Task<ValidationModel> RemoveCourse(int id)
        {
            try
            {
                if (id == 0)
                    return new ValidationModel { IsValid = false, Message = "Please provide a valid Id" };

                var course = _courseUnitOfWork.CourseRepository.GetById(id);
                if (course == null)
                    throw new Exception("Course does not exists.");

                _courseUnitOfWork.CourseRepository.Remove(course);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{course.Title} has been successfully remove." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public void Dispose()
        {
            _courseUnitOfWork.Dispose();
        }
    }
}
