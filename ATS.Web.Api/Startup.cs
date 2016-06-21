using System;
using System.Web.Configuration;
using System.Web.Http;
using ATS.Web.Api.Common;
using ATS.Web.Api.Security;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(ATS.Web.Api.Startup))]
namespace ATS.Web.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            MefWebApiConfig.Register();

            ConfigureOAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AllowInsecureHttp = Convert.ToBoolean(WebConfigurationManager.AppSettings["AllowInsecureHttp"]),
#if DEBUG
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
#else
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(15),
#endif
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
