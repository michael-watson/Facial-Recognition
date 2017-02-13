using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FacialRecognition.Backend.Startup))]

namespace FacialRecognition.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}