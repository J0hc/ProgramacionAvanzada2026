using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Registrar Context
builder.Services.AddDbContext<ProyectoFinalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ProyectoFinalContext>()
    .AddDefaultTokenProviders();

// Firebase Services
builder.Services.AddScoped<FirebaseStorageService>();
builder.Services.AddHttpClient<FirestoreService>();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();


// CONFIGURACIÓN FIREBASE 

// Ruta dentro de wwwroot 
var firebasePath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "wwwroot",
    "firebase",
    "firebase-key.json"
);

// Verificar que exista
if (!File.Exists(firebasePath))
{
    throw new Exception("❌ Falta el archivo firebase-key.json en wwwroot/firebase/. Ver README.");
}

// Leer contenido
var json = File.ReadAllText(firebasePath);

// Validar que no sea el ejemplo
var doc = JsonDocument.Parse(json);
var projectId = doc.RootElement.GetProperty("project_id").GetString();

if (projectId == "TU_PROJECT_ID")
{
    Console.WriteLine("Firebase no configurado. Usando modo sin conexión.");
}
else
{
    if (FirebaseApp.DefaultInstance == null)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(firebasePath)
        });
    }
}


// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// Roles y Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Administrador", "Estudiante" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    string adminEmail = "admin@correo.com";

    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            NombreCompleto = "Administrador"
        };

        await userManager.CreateAsync(user, "Admin123!");
        await userManager.AddToRoleAsync(user, "Administrador");
    }
}

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.Run();