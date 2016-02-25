using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Sales.DataModel.DbConfig;
using Sales.DataServices.ServiceClases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales.Web.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<CompanyService>().AsSelf().InstancePerDependency().WithParameter("context", new SalesDbContext());
            builder.RegisterType<MainAdminService>().AsSelf().InstancePerDependency().WithParameter("context", new SalesDbContext());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}