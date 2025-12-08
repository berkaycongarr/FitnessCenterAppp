using FitnessApp.Entities; // Senin projendeki Namespace (Entities klasörünün olduğu yer)
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVİS AYARLARI (Dependency Injection)
// ==========================================

// A) Veritabanı Bağlantısı (SQL Server)
builder.Services.AddDbContext<FitnessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// B) Authentication (Giriş Sistemi - Cookie Bazlı)
// Hoca'nın projesindeki mantığın aynısı, sadece modernize edildi.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "FitnessApp.Auth"; // Tarayıcıda görünecek çerez adı
        options.LoginPath = "/Account/Login";     // Giriş yapmamış kullanıcıyı buraya at
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz giriş denemesi (Örn: Üye admin sayfasına girmeye çalışırsa)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Oturum 60 dk sürsün
        options.SlidingExpiration = true; // Kullanıcı aktifse süreyi uzat
    });

// C) MVC Kontrolcüleri ve View'lar
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARE PIPELINE (İstek İşleme Sırası)
// ==========================================

// Hata yönetimi (Canlı ortamda hata detaylarını gizler)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // wwwroot klasörünü (CSS, JS, Resimler) dışarı açar

app.UseRouting();

// ÖNEMLİ: Bu iki satırın sırası KESİNLİKLE böyle olmalı.
app.UseAuthentication(); // 1. Kimlik Doğrulama (Kimsin?)
app.UseAuthorization();  // 2. Yetkilendirme (Girebilir misin?)

// Varsayılan Rota: Site açılınca Home/Index'e git.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();