using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext service with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session services (EKLENDÝ)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // 1 dakika inaktif kalýrsa oturum kapanýr
});



// Configure authentication (EKLENDÝ)
builder.Services.AddAuthentication("AdminScheme")
    .AddCookie("AdminScheme", options =>
    {
        options.LoginPath = "/Admin/Login"; // Login URL
        options.AccessDeniedPath = "/Admin/AccessDenied"; // Access denied URL
    });

//automapper ekleme
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use session middleware (EKLENDÝ)
app.UseSession();

// Use authentication and authorization middleware (EKLENDÝ)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
