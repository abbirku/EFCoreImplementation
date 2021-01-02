using Core;
using Infrastructure.Context;
using Infrastructure.DTO;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories
{
    public interface IStudentRegistrationRepository : IRepository<StudentRegistration, int, CourseContext>
    {
        List<RegistrationDTO> GetRegistrations();
    }

    public class StudentRegistrationRepository : Repository<StudentRegistration, int, CourseContext>, IStudentRegistrationRepository
    {
        public StudentRegistrationRepository(CourseContext context)
            : base(context)
        {

        }

        public List<RegistrationDTO> GetRegistrations()
        {
            var result = GetAll().Select(x => new RegistrationDTO
            {
                Id = x.Id,
                CourseTitle = x.Course.Title,
                StudentName = x.Student.Name,
                RegistrationDate = x.EnrollDate.ToString("dd/MM/yyyy"),
                IsPaymentComplete = x.IsPaymentComplete ? "Yes" : "No"
            }).ToList();

            return result;
        }
    }
}
