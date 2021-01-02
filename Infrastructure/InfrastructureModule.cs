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
                   .InstancePerLifetimeScope();

            //builder.RegisterGeneric(typeof(Repository<,,>)).As(typeof(IRepository<,,>))
            //    .InstancePerLifetimeScope();

            //Registering repositories
            builder.RegisterType<StudentRepository>().As<IStudentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CourseRepository>().As<ICourseRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StudentRegistrationRepository>().As<IStudentRegistrationRepository>()
                .InstancePerLifetimeScope();

            //Registering UnitOfWorks
            builder.RegisterType<CourseUnitOfWork>().As<ICourseUnitOfWork>()
                .InstancePerLifetimeScope();

            //Registering services
            builder.RegisterType<CourseRegistrationService>().As<ICourseRegistrationService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StudentService>().As<IStudentService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CourseService>().As<ICourseService>()
                .InstancePerLifetimeScope();

            //Registering type
            builder.RegisterType<RegistrationInfo>().AsSelf();

            base.Load(builder);
        }
    }
}
