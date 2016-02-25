using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using Sales.DataModel.DbConfig;
[assembly: OwinStartupAttribute(typeof(Sales.Web.Startup))]
namespace Sales.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
