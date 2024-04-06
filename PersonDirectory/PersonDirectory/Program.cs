using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using PersonDirectory.API.Middleware;
using PersonDirectory.Dependency.Configuration;
using PersonDirectory.Mapping;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;

namespace PersonDirectory.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers()
                   .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; })
                   .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ka-GE"),
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddRazorPages();

            builder.ConfigureDependency();

            var app = builder.Build();

            // This middleware detects the culture of each request and sets the current culture accordingly.
            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseErrorLoggingMiddleware();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}
