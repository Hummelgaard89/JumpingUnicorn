using JumpingUnicorn.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSceret"];
                googleOptions.ClaimActions.MapJsonKey("urn:google:profile", "link");
                googleOptions.ClaimActions.MapJsonKey("urn:google:image", "picture");
            });


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


            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}