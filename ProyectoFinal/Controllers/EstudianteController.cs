using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ProyectoFinal.Models;

[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    private readonly ProyectoFinalContext _context;

    public EstudianteController(ProyectoFinalContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Id del usuario logueado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Traer usuario con la carrera
        var estudiante = await _context.Users
       .Include(u => u.Carrera)
       .Include(u => u.Matriculas)
           .ThenInclude(m => m.Curso)
               .ThenInclude(c => c.Profesor)
       .FirstOrDefaultAsync(u => u.Id == userId);

        ViewBag.Nombre = estudiante?.NombreCompleto;
        ViewBag.Carrera = estudiante?.Carrera?.Nombre;
        ViewBag.TotalCursos = estudiante?.Matriculas?.Count() ?? 0;
        ViewBag.FotoUrl = estudiante?.FotoUrl;

        // Pasar
        return View(estudiante);
    }

    public async Task<IActionResult> Edit()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var estudiante = await _context.Users
            .Include(u => u.Carrera)
            .FirstOrDefaultAsync(u => u.Id == userId);

        ViewBag.Carreras = await _context.Carreras.ToListAsync();
        ViewBag.FotoUrl = estudiante?.FotoUrl;

        return View(estudiante);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model, IFormFile foto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var estudiante = await _context.Users
            .Include(u => u.Matriculas)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (estudiante == null)
            return NotFound();

        bool cambioCarrera = estudiante.CarreraId != model.CarreraId;

        estudiante.NombreCompleto = model.NombreCompleto;
        estudiante.Email = model.Email;
        estudiante.CarreraId = model.CarreraId;
        


        //  FOTO
        if (foto != null)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }

            estudiante.FotoUrl = "/images/users/" + fileName;
        }

        if (cambioCarrera)
        {
            var matriculas = await _context.Matriculas
                .Where(m => m.EstudianteId == userId)
                .ToListAsync();

            _context.Matriculas.RemoveRange(matriculas);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
        ViewBag.FotoUrl = estudiante.FotoUrl;
    }
}