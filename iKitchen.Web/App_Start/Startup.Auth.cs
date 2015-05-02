using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace iKitchen.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
                //clientId: "000000004C14FF77",
                //clientSecret: "a1fZUk4hKxb468Osq3DlzcWcTtQiYU5m");

            app.UseTwitterAuthentication(
               consumerKey: "8sXAaKlubhyK3ziZEbj4oLHWT",
               consumerSecret: "4Xgz044cmiWs53XBhvfa1vNMwRa4UlFbuZrVkCOVrDE7sPShfu");

            var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            {
                AppId = "1543874179228733",
                AppSecret = "2576bf58b588a6a1ec4e8b728c19115b",

                Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider
                {
                    OnAuthenticated = async context =>
                    {
                        //Get the access token from FB and store it in the database and
                        //use FacebookC# SDK to get more information about the user
                        context.Identity.AddClaim(
                        new System.Security.Claims.Claim("FacebookAccessToken",
                                             context.AccessToken));
                        //ClaimTypes.DateOfBirth, ctx.User["birthday"].ToString())
                        context.Identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.DateOfBirth, context.User["birthday"].ToString()));
                        context.Identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Gender, context.User["gender"].ToString()));
                    }
                }
            };
            facebookOptions.Scope.Add("email");
            facebookOptions.Scope.Add("user_birthday");
            facebookOptions.Scope.Add("user_location");
            app.UseFacebookAuthentication(facebookOptions);

            //app.UseGoogleAuthentication();
        }
    }
}