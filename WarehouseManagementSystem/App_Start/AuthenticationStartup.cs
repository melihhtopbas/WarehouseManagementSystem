using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WarehouseManagementSystem.Areas.Security;

[assembly: OwinStartup(typeof(WarehouseManagementSystem.App_Start.AuthenticationStartup))]

namespace WarehouseManagementSystem.App_Start
{
    public class AuthenticationStartup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                Provider = new OAuthBearerAuthenticationProvider
                {
                    OnValidateIdentity = context =>
                    {
                        // Add custom validation logic here
                        return Task.FromResult<object>(null);
                    }
                }
            });
            HttpConfiguration config = new HttpConfiguration();

            // Configure Web API
            WebApiConfig.Register(config);

            // Configure OAuth bearer token authentication middleware
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            // Configure Web API routes
            app.UseWebApi(config);
        }

    }
}
