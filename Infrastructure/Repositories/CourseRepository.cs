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
    public interface ICourseRepository : IRepository<Course, int, CourseContext>
    {
        IList<Course> GetCourses();
    }

    class CourseRepository : Repository<Course, int, CourseContext>, ICourseRepository
    {
        public CourseRepository(CourseContext dbContext)
            : base(dbContext)
        {
            
        }

        public IList<Course> GetCourses()
        {
            return GetAll();
        }
    }
}
