using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iKitchen.Web.Startup))]
namespace iKitchen.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
