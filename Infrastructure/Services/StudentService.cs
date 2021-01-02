using Infrastructure.BusinessObject;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IStudentService : IDisposable
    {
        Task<ValidationModel> EnrollStudent(StudentInfo studentInfo);
        Task<ValidationModel> UpdateStudentInfo(StudentInfo studentInfo);
        Task<ValidationModel> RemoveStudent(int id);
    }

    public class StudentService : IStudentService
    {
        public readonly ICourseUnitOfWork _courseUnitOfWork;

        public StudentService(ICourseUnitOfWork courseUnitOfWork)
        {
            _courseUnitOfWork = courseUnitOfWork;
        }

        public async Task<ValidationModel> EnrollStudent(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Add(studentInfo.Student);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{studentInfo.Student.Name} has been successfully enrolled." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel> UpdateStudentInfo(StudentInfo studentInfo)
        {
            try
            {
                var validation = studentInfo.IsValid();
                if (!validation.IsValid)
                    return new ValidationModel { IsValid = false, Message = validation.Message };

                _courseUnitOfWork.StudentRepository.Edit(studentInfo.Student);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{studentInfo.Student.Name} data has been successfully updated." };
            }
            catch (Exception ex)
            {
                //Implement serilog for logging the error message
                return new ValidationModel { IsValid = false, Message = ex.Message };
            }
        }

        public async Task<ValidationModel> RemoveStudent(int id)
        {
            try
            {
                if (id == 0)
                    return new ValidationModel { IsValid = false, Message = "Please provide a valid Id" };

                var student = _courseUnitOfWork.StudentRepository.GetById(id);
                if (student == null)
                    throw new Exception("Student does not exists.");

                _courseUnitOfWork.StudentRepository.Remove(student);
                await _courseUnitOfWork.SaveChangesAsync();

                return new ValidationModel { IsValid = true, Message = $"{student.Name} has been successfully remove." };
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
