using Core;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UnitOfWorks
{
    public interface ICourseUnitOfWork : IUnitOfWork
    {
        public IStudentRepository StudentRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public IStudentRegistrationRepository StudentRegistrationRepository { get; set; }
    }
}
