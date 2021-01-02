using Infrastructure.BusinessObject;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models
{
    public class CourseModel : IDisposable
    {
        private readonly ICourseUnitOfWork _courseUnitOfWork;
        private readonly ICourseService _courseService;

        public CourseModel(ICourseUnitOfWork courseUnitOfWork, ICourseService courseService)
        {
            _courseUnitOfWork = courseUnitOfWork;
            _courseService = courseService;
        }

        public CourseViewModel CreateCourseViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedCourse = new Course { Title = string.Empty, SeatCount = 0, Fee = 0 };
            if (id != 0)
            {
                selectedCourse = _courseUnitOfWork.CourseRepository.GetById(id);
            }

            var model = new CourseViewModel
            {
                Courses = _courseUnitOfWork.CourseRepository.GetCourses(),
                SelectedCourse = selectedCourse,
                Validation = new ValidationModel { IsValid = isValid, Message = message }
            };

            return model;
        }

        public void Dispose()
        {
            _courseService.Dispose();
        }
    }

    public class CourseViewModel
    {
        public Course SelectedCourse { get; set; }
        public IList<Course> Courses { get; set; }
        public ValidationModel Validation { get; set; }
    }
}
