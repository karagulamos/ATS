using System;
using System.ComponentModel.Composition;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Core.Bootstrapper;
using Microsoft.Owin.Security.OAuth;

namespace ATS.Web.Api.Security
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        private IUserAuthenticationService _userAuthenticationService;

        public SimpleAuthorizationServerProvider()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                if (!_userAuthenticationService.ValidateUserCredentials(context.UserName, context.Password))
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Email, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, _userAuthenticationService.GetUserId(context.UserName)));

                foreach (var role in _userAuthenticationService.GetUserRoles(context.UserName))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "Internal Server Error Validating User : " + ex);
            }
        }
    }
}