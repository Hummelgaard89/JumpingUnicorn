using JumpingUnicorn.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Google.Cloud.Firestore;
using JumpingUnicorn.Database;
using JumpingUnicorn.Policy;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JumpingUnicorn
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("JumpingUnicornContextConnection") ?? throw new InvalidOperationException("Connection string 'JumpingUnicornContextConnection' not found.");
            var services = builder.Services;
            var configuration = builder.Configuration;

            // Add services to the container.
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<AvatarService>();
            services.AddSingleton<FirebaseContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.OnAppendCookie = cookieContext =>
                  CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                  CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options => 
                {
                    options.Cookie.Name = "LogInCookie";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/Leaderboard";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                });


            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSceret"];
                googleOptions.ClaimActions.MapJsonKey("urn:google:profile", "link");
                googleOptions.ClaimActions.MapJsonKey("urn:google:image", "picture");
            });

            services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UsernameSet", policy =>
                {
                    policy.Requirements.Add(new SetUsernameRequirment(true));
                });
            });

            services.AddScoped<IAuthorizationHandler, SetUsernameRequirmentHandler>();


            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Use(async (context,next) => {
                const string CookieName = "avatar";
                if (context.Request.Cookies[CookieName] == null || !int.TryParse(context.Request.Cookies[CookieName], out int res))
                {
                    var cookieOptions = new CookieOptions
                    {
                        // Set the secure flag, which Chrome's changes will require for SameSite none.
                        // Note this will also require you to be running on HTTPS
                        Secure = true,

                        // Set the cookie to HTTP only which is good practice unless you really do need
                        // to access it client side in scripts.
                        HttpOnly = true,

                        // Add the SameSite attribute, this will emit the attribute with a value of none.
                        // To not emit the attribute at all set the SameSite property to SameSiteMode.Unspecified.
                        SameSite = SameSiteMode.Unspecified
                    };

                    // Add the cookie to the response cookie collection
                    context.Response.Cookies.Append(CookieName, "0", cookieOptions);
                }
                await next(context);
            });

            app.Run();
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (SameSite.BrowserDetection.DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }
    }
}