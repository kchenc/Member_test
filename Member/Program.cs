using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Member.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MemberContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MemberContext") ?? throw new InvalidOperationException("Connection string 'MemberContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(1800);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
