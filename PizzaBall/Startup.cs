using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PizzaBall.Startup))]
namespace PizzaBall
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
