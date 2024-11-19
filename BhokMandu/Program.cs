using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using BhokMandu.Data;
using BhokMandu.Models;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<BhokManduContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("BhokManduContext")
		?? throw new InvalidOperationException("Connection string 'BhokManduContext' not found.")));

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add distributed memory cache and session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Configure authentication with cookies
builder.Services.AddAuthentication("Cookies")
	.AddCookie("Cookies", options =>
	{
		options.Cookie.Name = "BhokManduAuth"; // Unique name for your cookie
		options.Cookie.HttpOnly = true;
		options.Cookie.IsEssential = true;
		options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Match or extend session timeout
		options.LoginPath = "/Account/Login"; // Default Login Path for Users
		options.AccessDeniedPath = "/Home/AccessDenied";

		// Add logic for admin login redirection
		options.Events.OnRedirectToLogin = context =>
		{
			var requestPath = context.Request.Path.Value;

			// Redirect to Admin Login if the request path is under "/Admin"
			if (requestPath != null && requestPath.StartsWith("/Admin"))
			{
				context.Response.Redirect("/Admin/AdminLogin");
			}
			else
			{
				context.Response.Redirect("/Account/Login");
			}
			return Task.CompletedTask;
		};
	});

// Add authorization policies for Admins and Users
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
	options.AddPolicy("User", policy => policy.RequireAuthenticatedUser());
});

var app = builder.Build();

// Seed database (optional)
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	SeedData.Initialize(services);  // Ensure SeedData.Initialize is implemented in your project
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseHsts(); // Use HSTS for production environments
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session and authentication/authorization middleware
app.UseSession();
app.UseAuthentication();  // Ensure authentication is configured before authorization
app.UseAuthorization();

// Define default route
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();