using Autofac;
using Infrastructure.BusinessObject;
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
    public class CourseModel : IDisposable
    {
        private readonly ICourseService _courseService;

        public CourseModel()
        {
            _courseService = Startup.AutofacContainer.Resolve<ICourseService>();
        }

        internal CourseViewModel<Course> CreateCourseViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedCourse = new Course { Title = string.Empty, SeatCount = 0, Fee = 0 };
            if (id != 0)
            {
                var courseValidationModel = _courseService.GetCourseById(id);
                if (!courseValidationModel.IsValid)
                    return new CourseViewModel<Course> { Validation = courseValidationModel };

                selectedCourse = courseValidationModel.Data;
            }

            var model = new CourseViewModel<Course>
            {
                Courses = _courseService.GetCourses(),
                SelectedCourse = selectedCourse,
                Validation = new ValidationModel<Course> { IsValid = isValid, Data = selectedCourse, Message = message }
            };

            return model;
        }

        internal async Task<ValidationModel<Course>> Createcourse(CourseInfo courseInfo)
        {
            return await _courseService.Createcourse(courseInfo);
        }

        internal async Task<ValidationModel<Course>> UpdateCourseInfo(CourseInfo courseInfo)
        {
            return await _courseService.UpdateCourseInfo(courseInfo);
        }

        internal async Task<ValidationModel<Course>> RemoveCourse(int id)
        {
            return await _courseService.RemoveCourse(id);
        }

        public void Dispose()
        {
            _courseService.Dispose();
        }
    }

    public class CourseViewModel<T>
    {
        public Course SelectedCourse { get; set; }
        public IList<Course> Courses { get; set; }
        public ValidationModel<T> Validation { get; set; }
    }
}
