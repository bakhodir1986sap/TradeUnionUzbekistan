using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KasabaKIAT.Startup))]
namespace KasabaKIAT
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}