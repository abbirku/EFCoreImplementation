using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Infrastructure.BusinessObject;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index(int id = 0, bool isValid = false, string message = "")
        {
            try
            {
                var courseModel = Startup.AutofacContainer.Resolve<CourseModel>();
                var model = courseModel.CreateCourseViewModel(id, isValid, message);

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Course selectedCourse)
        {
            try
            {
                var courseModel = Startup.AutofacContainer.Resolve<CourseModel>();
                var result = await courseModel.Createcourse(new CourseInfo
                {
                    Course = selectedCourse
                });

                if (result.IsValid)
                    return RedirectToAction("Index", "Course", new { isValid = result.IsValid, message = result.Message });
                else
                    return RedirectToAction("Index", "Error", new { message = result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course selectedCourse)
        {
            try
            {
                var courseModel = Startup.AutofacContainer.Resolve<CourseModel>();
                var result = await courseModel.UpdateCourseInfo(new CourseInfo
                {
                    Course = selectedCourse
                });

                if (result.IsValid)
                    return RedirectToAction("Index", "Course", new { isValid = result.IsValid, message = result.Message });
                else
                    return RedirectToAction("Index", "Error", new { message = result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var courseModel = Startup.AutofacContainer.Resolve<CourseModel>();
                var result = await courseModel.RemoveCourse(id);

                return Json(new { result.IsValid, result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }
    }
}
