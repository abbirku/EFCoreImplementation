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
        
    }

    public class StudentRepository : Repository<Student, int, CourseContext>, IStudentRepository
    {
        public StudentRepository(CourseContext dbContext)
            : base(dbContext)
        {

        }
    }
}
