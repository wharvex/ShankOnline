using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShankOnline.Data;

namespace ShankOnline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            const string csFieldName = "ConnectionStrings__ShankOnlineDb";
            var connectionString = builder.Configuration[csFieldName];
            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        connectionString,
                        options2 => options2.EnableRetryOnFailure()
                    )
            );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder
                .Services.AddDefaultIdentity<IdentityUser>(
                    options => options.SignIn.RequireConfirmedAccount = true
                )
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                )
                .WithStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }
    }
}
