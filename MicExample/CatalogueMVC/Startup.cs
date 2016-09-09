using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CatalogueMVC.Startup))]
namespace CatalogueMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
