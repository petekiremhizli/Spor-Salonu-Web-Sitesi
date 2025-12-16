using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FitnessCenterProject.Services; // AIService'in bulunduğu namespace

var builder = WebApplication.CreateBuilder(args);

/// MVC
builder.Services.AddControllersWithViews();

/// DB CONTEXT
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ⭐️ AIService Kaydı: HomeController'a enjekte edilebilmesi için eklendi
builder.Services.AddScoped<AIService>(); // Tek eksik olan ve eklenen satır burasıdır.

/// ✅ IDENTITY
builder.Services.AddIdentity<Uye, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

/// ✅ COOKIE AYARLARI
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

/// ✅ ROLE + ADMIN SEED (DOĞRU YER)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Uye>>();

    // Roller
    string[] roles = { "Admin", "Uye" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Admin kullanıcı
    string adminEmail = "g231210030@sakarya.edu.tr";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new Uye
        {
            UserName = adminEmail,
            Email = adminEmail,
            Ad = "Admin",
            Soyad = "User",
            Telefon = "0000000000",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Sau123456!");

        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

/// MIDDLEWARE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

/// ❗ SIRASI ÇOK ÖNEMLİ
app.UseAuthentication();
app.UseAuthorization();

/// ROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();