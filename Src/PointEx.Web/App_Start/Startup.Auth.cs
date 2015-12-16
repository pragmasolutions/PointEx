using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using PointEx.Security.Managers;
using PointEx.Security.Model;
using PointEx.Web.Models;

namespace PointEx.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            var facebookAuthenticationOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions();
            facebookAuthenticationOptions.Scope.Add("email");
            facebookAuthenticationOptions.Scope.Add("user_birthday");
            //facebookAuthenticationOptions.Scope.Add("user_last_name");
            //facebookAuthenticationOptions.Scope.Add("user_location");
            facebookAuthenticationOptions.AppId = "446501508875036";
            facebookAuthenticationOptions.AppSecret = "0a61f56bcb59bdc4f5be641f7f24b0ba";
            facebookAuthenticationOptions.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = async context =>
                {
                    context.Identity.AddClaim( new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                }
            };

            app.UseFacebookAuthentication(facebookAuthenticationOptions);

            //string XmlSchemaString = "www.w3.org/.../XMLSchema";
            //FacebookAuthenticationOptions options = new FacebookAuthenticationOptions();
            //options.Scope.Add("email");
            //options.AppId = "446501508875036";
            //options.AppSecret = "0a61f56bcb59bdc4f5be641f7f24b0ba";
            //options.Provider = new FacebookAuthenticationProvider()
            //{
            //    OnAuthenticated = (context) =>
            //    {
            //        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
            //        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:email", context.Email, XmlSchemaString, "Facebook"));
            //        foreach (var x in context.User)
            //        {
            //            string claimType = string.Format("urn:facebook:{0}", x.Key);
            //            string claimValue = x.Value.ToString();
            //            if (!context.Identity.HasClaim(claimType, claimValue))
            //            {
            //                context.Identity.AddClaim(new Claim(claimType, claimValue, XmlSchemaString, "Facebook"));
            //            }
            //        }
            //        return Task.FromResult(0);
            //    }
            //};
            //options.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            //app.UseFacebookAuthentication(options);

            //app.UseFacebookAuthentication(
            //   appId: "446501508875036",
            //   appSecret: "0a61f56bcb59bdc4f5be641f7f24b0ba");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}