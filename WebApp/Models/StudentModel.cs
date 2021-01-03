using Infrastructure.DTO;
using Infrastructure.Entities;
using Infrastructure.BusinessObject;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace WebApp.Models
{
    public class StudentModel : IDisposable
    {
        private readonly IStudentService _studentService;

        public StudentModel()
        {
            _studentService = Startup.AutofacContainer.Resolve<IStudentService>();
        }

        internal StudentViewModel<Student> CreateStudentViewModel(int id = 0, bool isValid = false, string message = "")
        {
            var selectedStudent = new Student { Name = string.Empty, DateOfBirth = DateTime.Now };
            if (id != 0)
            {
                var studentValidationModel = _studentService.GetStudentById(id);
                if (!studentValidationModel.IsValid)
                    return new StudentViewModel<Student> { Validation = studentValidationModel };

                selectedStudent = studentValidationModel.Data;
            }


            var model = new StudentViewModel<Student>
            {
                Students = _studentService.GetStudents(),
                SelectedStudent = selectedStudent,
                Validation = new ValidationModel<Student> { IsValid = isValid, Data = selectedStudent, Message = message }
            };

            return model;
        }

        internal ValidationModel<Student> EnrollStudent(StudentInfo studentInfo)
        {
            return _studentService.EnrollStudent(studentInfo);
        }

        internal ValidationModel<Student> UpdateStudentInfo(StudentInfo studentInfo)
        {
            return _studentService.UpdateStudentInfo(studentInfo);
        }

        internal ValidationModel<Student> RemoveStudent(int id)
        {
            return _studentService.RemoveStudent(id);
        }

        public void Dispose()
        {
            _studentService.Dispose();
        }
    }

    public class StudentViewModel<T>
    {
        public Student SelectedStudent { get; set; }
        public IList<StudentDTO> Students { get; set; }
        public ValidationModel<T> Validation { get; set; }
    }
}
