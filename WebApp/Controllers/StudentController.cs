using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index(int id = 0, bool isValid = false, string message = "")
        {
            try
            {
                var studentModel = Startup.AutofacContainer.Resolve<StudentModel>();
                var model = studentModel.CreateStudentViewModel(id, isValid, message);

                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Add(Student selectedStudent)
        {
            try
            {
                var studentModel = Startup.AutofacContainer.Resolve<StudentModel>();
                var result = studentModel.EnrollStudent(new StudentInfo { Student = selectedStudent });

                if (result.IsValid)
                    return RedirectToAction("Index", "Student", new { isValid = result.IsValid, message = result.Message });
                else
                    return RedirectToAction("Index", "Error", new { message = result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Edit(Student selectedStudent)
        {
            try
            {
                var studentModel = Startup.AutofacContainer.Resolve<StudentModel>();
                var result = studentModel.UpdateStudentInfo(new StudentInfo { Student = selectedStudent });

                if (result.IsValid)
                    return RedirectToAction("Index", "Student", new { isValid = result.IsValid, message = result.Message });
                else
                    return RedirectToAction("Index", "Error", new { message = result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var studentModel = Startup.AutofacContainer.Resolve<StudentModel>();
                var result = studentModel.RemoveStudent(id);

                return Json(new { result.IsValid, result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }
    }
}
