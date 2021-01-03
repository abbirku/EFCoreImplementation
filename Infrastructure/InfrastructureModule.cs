using Autofac;
using Core;
using Infrastructure.BusinessObject;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWorks;

namespace Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public InfrastructureModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CourseContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope(); ;

            //builder.RegisterGeneric(typeof(Repository<,,>)).As(typeof(IRepository<,,>))
            //    .InstancePerLifetimeScope();

            //Registering repositories
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>();
            builder.RegisterType<StudentRegistrationRepository>().As<IStudentRegistrationRepository>();

            //Registering UnitOfWorks
            builder.RegisterType<CourseUnitOfWork>().As<ICourseUnitOfWork>();

            //Registering services
            builder.RegisterType<CourseRegistrationService>().As<ICourseRegistrationService>();
            builder.RegisterType<StudentService>().As<IStudentService>();
            builder.RegisterType<CourseService>().As<ICourseService>();

            //Registering type
            builder.RegisterType<RegistrationInfo>().AsSelf();

            base.Load(builder);
        }
    }
}
