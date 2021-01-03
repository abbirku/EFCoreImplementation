using Infrastructure.BusinessObject;
using Infrastructure.DTO;
using Infrastructure.Entities;
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
        ValidationModel<Student> GetStudentById(int id);
        ValidationModel<Student> EnrollStudent(StudentInfo studentInfo);
        ValidationModel<Student> UpdateStudentInfo(StudentInfo studentInfo);
        ValidationModel<Student> RemoveStudent(int id);
        IList<StudentDTO> GetStudents();
    }

    public class StudentService : IStudentService
    {
        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public StudentService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public ValidationModel<Student> GetStudentById(int id)
        {
            try
            {
                var student = _courseUnitOfWork.StudentRepository.GetById(id);
                if (student == null)
                    throw new Exception($"Student does not exists with {id} Id");

                return new ValidationModel<Student> { IsValid = true, Data = student, Message = "Data found" };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Student> { IsValid = false, Message = ex.Message };
            }
        }

        public ValidationModel<Student> EnrollStudent(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel<Student> { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Add(studentInfo.Student);
                _courseUnitOfWork.SaveChanges();

                return new ValidationModel<Student> { IsValid = true, Data = studentInfo.Student, Message = $"{studentInfo.Student.Name} has been successfully enrolled." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Student> { IsValid = false, Message = ex.Message };
            }
        }

        public ValidationModel<Student> UpdateStudentInfo(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel<Student> { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Edit(studentInfo.Student);
                _courseUnitOfWork.SaveChanges();

                return new ValidationModel<Student> { IsValid = true, Data = studentInfo.Student ,Message = $"{studentInfo.Student.Name} data has been successfully updated." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Student> { IsValid = false, Message = ex.Message };
            }
        }

        public ValidationModel<Student> RemoveStudent(int id)
        {
            try
            {
                if (id == 0)
                    return new ValidationModel<Student> { IsValid = false, Message = "Please provide a valid Id" };

                var student = _courseUnitOfWork.StudentRepository.GetById(id);
                if (student == null)
                    throw new Exception("Student does not exists.");

                _courseUnitOfWork.StudentRepository.Remove(student);
                _courseUnitOfWork.SaveChanges();

                return new ValidationModel<Student> { IsValid = true, Data = student, Message = $"{student.Name} has been successfully remove." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel<Student> { IsValid = false, Message = ex.Message };
            }
        }

        public IList<StudentDTO> GetStudents()
        {
            return _courseUnitOfWork.StudentRepository.GetStudents();
        }

        public void Dispose()
        {
            _courseUnitOfWork.Dispose();
        }
    }
}
