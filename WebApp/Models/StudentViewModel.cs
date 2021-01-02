using Infrastructure.DTO;
using Infrastructure.Entities;
using Infrastructure.BusinessObject;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class StudentModel : IDisposable
    {
        private readonly ICourseUnitOfWork _courseUnitOfWork;
        private readonly IStudentService _studentService;

        public StudentModel(ICourseUnitOfWork courseUnitOfWork, IStudentService studentService)
        {
            _courseUnitOfWork = courseUnitOfWork;
            _studentService = studentService;
        }

        public StudentViewModel CreateStudentViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedStudent = new Student { Name = string.Empty, DateOfBirth = DateTime.Now };
            if (id != 0)
                selectedStudent = _courseUnitOfWork.StudentRepository.GetById(id);

            var model = new StudentViewModel
            {
                Students = _courseUnitOfWork.StudentRepository.GetStudents(),
                SelectedStudent = selectedStudent,
                Validation = new ValidationModel { IsValid = isValid, Message = message }
            };

            return model;
        }

        public void Dispose()
        {
            _studentService.Dispose();
        }
    }

    public class StudentViewModel
    {
        public Student SelectedStudent { get; set; }
        public List<StudentDTO> Students { get; set; }
        public ValidationModel Validation { get; set; }
    }
}
