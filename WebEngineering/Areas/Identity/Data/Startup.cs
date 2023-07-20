using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebEngineering.Models;
using Microsoft.AspNetCore.Identity;
using WebEngineering.Data;
using WebEngineering.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllersWithViews();
        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityContextConnection")));

        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Enable logging for the authentication process
        app.Use(async (context, next) =>
        {
            var result = await context.AuthenticateAsync();
            if (result?.Principal != null)
            {
                logger.LogInformation("User authenticated: {Username}", result.Principal.Identity.Name);
            }

            await next();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}
