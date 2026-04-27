using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinal.Models;


namespace ProyectoFinal.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ProyectoFinalContext _context;

        public CuentaController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ProyectoFinalContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        
        // GET: Registrarse
       
        [HttpGet]
        public IActionResult Register()
        {
            var model = new Registro
            {
                Carreras = _context.Carreras
                    .OrderBy(c => c.Nombre)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    })
                    .ToList()
            };

            return View(model);
        }

        // POST: Registrarse
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registro model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar dropdown en caso de que falle
                model.Carreras = _context.Carreras
                    .OrderBy(c => c.Nombre)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    })
                    .ToList();
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                EmailConfirmed = true,
                CarreraId = model.CarreraId // Asociar la carrera
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Rol Estudiante
                await _userManager.AddToRoleAsync(user, "Estudiante");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Estudiante");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            // Recargar dropdown
            model.Carreras = _context.Carreras
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            return View(model);
        }

        // GET: Login

        [HttpGet]
        public IActionResult Login() => View();


        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            // 1. Validaciones del modelo 
            if (!ModelState.IsValid)
                return View(model);

            // Login
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            // Login correcto
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                
                if (user == null)
                {
                    ModelState.AddModelError("", "Email o contraseña incorrecta");
                    return View(model);
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrador"))
                    return RedirectToAction("Dashboard", "Admin");
                else
                    return RedirectToAction("Index", "Estudiante");
            }

            //Manejo de errores 
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Cuenta bloqueada. Intente más tarde.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Acceso no permitido.");
            }
            else
            {
                
                ModelState.AddModelError("", "Email o contraseña incorrecta");
            }

            return View(model);
        }

        // POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}