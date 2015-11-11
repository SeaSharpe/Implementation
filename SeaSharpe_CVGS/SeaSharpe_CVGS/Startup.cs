using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeaSharpe_CVGS.Startup))]
namespace SeaSharpe_CVGS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
