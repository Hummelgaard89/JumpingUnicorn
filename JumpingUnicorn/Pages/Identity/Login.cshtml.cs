using JumpingUnicorn.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;

namespace JumpingUnicorn.Pages.Identity
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly FirebaseContext _firebaseContext;

        public LoginModel(FirebaseContext firebaseContext)
        {
            _firebaseContext = firebaseContext;
        }

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            string provider = "Google";
            // Request a redirect to the external login provider.
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login",
                pageHandler: "Callback",
                values: new { returnUrl }),
            };
            return new ChallengeResult(provider, authenticationProperties);
        }
        public async Task<IActionResult> OnGetCallbackAsync(
            string returnUrl = null, string remoteError = null)
        {

            string redirctURL = "/";
            // Get the information about the user from the external login provider
            var GoogleUser = this.User.Identities.FirstOrDefault();
            if (GoogleUser.IsAuthenticated)
            {
                string userId = GoogleUser.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };

                if(GoogleUser.HasClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "106521800893253693222"))
                {
                    GoogleUser.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    GoogleUser.AddClaim(new Claim(ClaimTypes.Role, "User"));
                }
                
                if (!await _firebaseContext.DoesUserExistAsync(userId))
                {
                    string id = userId;
                    string username = "";
                    string avatar = GoogleUser.Claims.Where(x => x.Type == "urn:google:image").FirstOrDefault().Value;

                    await _firebaseContext.AddUserAsync(new Data.User(username,avatar, id));
                }

                if (!await _firebaseContext.DoesUserHasUsername(userId))
                {
                    redirctURL = "/RegisterUsername";
                }
                else
                {
                    GoogleUser.AddClaim(new Claim("UsernameSet", "True"));
                }

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(GoogleUser),
                authProperties);
            }
            return LocalRedirect(redirctURL);
        }
    }
}
