using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RedSocialNetCore.Data;
using RedSocialNetCore.Helpers;
using RedSocialNetCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<RepositoryInicio>();
builder.Services.AddTransient<RepositoryHome>();
builder.Services.AddTransient<RepositoryPerfil>();
builder.Services.AddTransient<RepositoryEditarPerfil>();
builder.Services.AddTransient<RepositoryPublicacion>();
builder.Services.AddTransient<RepositorySeguidores>();
builder.Services.AddTransient<RepositoryBuscador>();

builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddTransient<HelperCryptography>();

string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<ContextApp>(options => options.UseSqlServer(connectionString));

// COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { });

builder.Services.AddSingleton<IHttpContextAccessor
    , HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});




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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
