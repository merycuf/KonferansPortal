using KonferansPortal;
using KonferansPortal.Data;
using KonferansPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KonferansPortal")));

builder.Services.AddIdentity<Uye, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthorizationHandler, IsKatilimciHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsEgitmenHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsEgitmenOrKatilimciHandler>();

// Add Authentication and Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsKatilimci", policy =>
        policy.Requirements.Add(new IsKatilimciRequirement()));
    options.AddPolicy("IsEgitmen", policy =>
        policy.Requirements.Add(new IsEgitmenRequirement()));
    options.AddPolicy("IsEgitmenOrKatilimci", policy =>
        policy.Requirements.Add(new IsEgitmenOrKatilimciRequirement()));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Uye>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<AppDbContext>();
    await SeedData.Initialize(userManager, roleManager, context);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();