using Core;
using Infrastructure.Context;
using Infrastructure.Entities;
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

    public class CourseUnitOfWork : UnitOfWork, ICourseUnitOfWork
    {
        public IStudentRepository StudentRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public IStudentRegistrationRepository StudentRegistrationRepository { get; set; }

        public CourseUnitOfWork(CourseContext dbContext,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IStudentRegistrationRepository studentRegistrationRepository)
            : base(dbContext)
        {
            StudentRepository = studentRepository;
            CourseRepository = courseRepository;
            StudentRegistrationRepository = studentRegistrationRepository;
        }
    }
}
