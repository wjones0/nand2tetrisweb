using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nand2TetrisWeb.Startup))]
namespace Nand2TetrisWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
