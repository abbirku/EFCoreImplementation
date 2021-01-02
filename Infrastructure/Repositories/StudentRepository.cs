using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Context;
using Infrastructure.DTO;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IStudentRepository : IRepository<Student, int, CourseContext>
    {
        List<StudentDTO> GetStudents();
    }

    public class StudentRepository : Repository<Student, int, CourseContext>, IStudentRepository
    {
        public StudentRepository(CourseContext dbContext)
            : base(dbContext)
        {

        }

        public List<StudentDTO> GetStudents()
        {
            var studentList = GetAll().Select(x => new StudentDTO
            {
                Id = x.Id,
                Name = x.Name,
                DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy")
            }).ToList();

            return studentList;
        }
    }
}
