﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ICourseRegistrationService _courseRegistrationService;

        public RegistrationController(ICourseRegistrationService courseRegistrationService)
        {
            _courseRegistrationService = courseRegistrationService;
        }

        public IActionResult Index(int id = 0, bool isValid = false, string message = "")
        {
            var registrationModel = Startup.AutofacContainer.Resolve<RegistrationModel>();
            var model = registrationModel.CreateStudentViewModel(id, isValid, message);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentRegistration selectedRegistration)
        {
            try
            {
                var registrationInfoModel = Startup.AutofacContainer.Resolve<RegistrationInfo>();
                registrationInfoModel.StudentRegistration = selectedRegistration;

                var result = await _courseRegistrationService.RegisterStudentAsync(registrationInfoModel);

                if (result.IsValid)
                    return RedirectToAction("Index", "Registration", new { isValid = result.IsValid, message = result.Message });
                else
                    return RedirectToAction("Index", "Error", new { message = result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentRegistration selectedRegistration)
        {
            try
            {
                var registrationInfoModel = Startup.AutofacContainer.Resolve<RegistrationInfo>();
                registrationInfoModel.StudentRegistration = selectedRegistration;

                var result = await _courseRegistrationService.UpdateRegisteration(registrationInfoModel);

                if (result.IsValid)
                    return RedirectToAction("Index", "Registration", new { isValid = result.IsValid, message = result.Message });
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
                var result = await _courseRegistrationService.DeletRegisteration(id);

                return Json(new { result.IsValid, result.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { message = ex.Message });
            }
        }
    }
}
