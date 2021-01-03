using Infrastructure.BusinessObject;
using Infrastructure.DTO;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IStudentService : IDisposable
    {
        ValidationModel EnrollStudent(StudentInfo studentInfo);
        ValidationModel UpdateStudentInfo(StudentInfo studentInfo);
        ValidationModel RemoveStudent(int id);
        IList<StudentDTO> GetStudents();
    }

    public class StudentService : IStudentService
    {
        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public StudentService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public ValidationModel EnrollStudent(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Add(studentInfo.Student);
                _courseUnitOfWork.SaveChanges();

                return new ValidationModel { IsValid = true, Message = $"{studentInfo.Student.Name} has been successfully enrolled." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public ValidationModel UpdateStudentInfo(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Edit(studentInfo.Student);
                _courseUnitOfWork.SaveChanges();

                var test = _courseUnitOfWork.StudentRepository.GetAll();

                return new ValidationModel { IsValid = true, Message = $"{studentInfo.Student.Name} data has been successfully updated." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public ValidationModel RemoveStudent(int id)
        {
            try
            {
                if (id == 0)
                    return new ValidationModel { IsValid = false, Message = "Please provide a valid Id" };

                var student = _courseUnitOfWork.StudentRepository.GetById(id);
                if (student == null)
                    throw new Exception("Student does not exists.");

                _courseUnitOfWork.StudentRepository.Remove(student);
                _courseUnitOfWork.SaveChanges();

                return new ValidationModel { IsValid = true, Message = $"{student.Name} has been successfully remove." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public IList<StudentDTO> GetStudents()
        {
            var studentList = _courseUnitOfWork.StudentRepository.GetAll().Select(x => new StudentDTO
            {
                Id = x.Id,
                Name = x.Name,
                DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy")
            }).ToList();

            return studentList;
        }

        public void Dispose()
        {
            _courseUnitOfWork.Dispose();
        }
    }
}
