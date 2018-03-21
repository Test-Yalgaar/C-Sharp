using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GNM.Startup))]
namespace GNM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
