using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PushNotifyApp.Startup))]
namespace PushNotifyApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
