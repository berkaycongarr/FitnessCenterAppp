using FitnessCenter.Data; // <-- senin DbContext sınıfının olacağı klasör (oluşturacağız)
using FitnessCenterApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ MVC Servislerini ekle
builder.Services.AddControllersWithViews();

// 2️⃣ Entity Framework Core - SQL Server bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3️⃣ Session (oturum) yönetimi aktif et
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi (30 dk)
    options.Cookie.HttpOnly = true;                 // Tarayıcıdan sadece HTTP erişimi
    options.Cookie.IsEssential = true;              // GDPR için zorunlu çerez olarak işaretle
});

// 4️⃣ Authentication (Kimlik Doğrulama) ekle
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";      // Giriş yapılmadıysa yönlendirilecek sayfa
        options.LogoutPath = "/Account/Logout";    // Çıkış sonrası yönlendirme
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim
    });

var app = builder.Build();

// 5️⃣ Middleware Pipeline (istek akışı)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 6️⃣ Authentication ve Authorization middleware’leri
app.UseAuthentication();
app.UseAuthorization();

// 7️⃣ Session middleware’i
app.UseSession();

// 8️⃣ Varsayılan yönlendirme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
