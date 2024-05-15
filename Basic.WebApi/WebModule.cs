using Autofac;
using Autofac.Core;
using Basic.DataAccesEF.UnitOfWork;
using Basic.Domain.Interfaces;
using Basic.WebApi.Model;

namespace Basic.WebApi
{
    //  Autofac Dependency Injection
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterType<CourseModel>.As<ICourseModel>().In
           // builder.RegisterType<Course>().As<ICourse>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
              .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
