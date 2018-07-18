using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FashionableFitandFlawless.Startup))]
namespace FashionableFitandFlawless
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
