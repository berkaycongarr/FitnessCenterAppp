using FitnessApp.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Servisi
builder.Services.AddDbContext<FitnessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Kimlik Doğrulama (Cookie)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddControllersWithViews();

// ============================================================
// HATA BURADAYDI: app değişkeni sadece bir kere tanımlanmalı.
// ============================================================
var app = builder.Build();

// DİL VE ONDALIK SAYI AYARI (Nokta kullanımı için)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// 3. Otomatik Admin Ekleme (Seeding)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();

    // Admin yoksa ekle
    if (!context.Users.Any(u => u.Role == "admin"))
    {
        context.Users.Add(new User
        {
            Username = "Admin Yonetici",
            Email = "ogrencinumarasi@sakarya.edu.tr",
            Password = "sau",
            Role = "admin",
            CreatedAt = DateTime.Now
        });
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ÖNCE
app.UseAuthorization();  // SONRA

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();