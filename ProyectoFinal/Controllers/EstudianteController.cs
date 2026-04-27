using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    private readonly ProyectoFinalContext _context;
    private readonly FirebaseStorageService _firebase;
    

    // Firebas
    public EstudianteController(ProyectoFinalContext context, FirebaseStorageService firebase)
    {
        _context = context;
        _firebase = firebase;
    }

    // INDEX
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var estudiante = await _context.Users
            .Include(u => u.Carrera)
            .Include(u => u.Matriculas)
                .ThenInclude(m => m.Curso)
                    .ThenInclude(c => c.Profesor)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (estudiante == null)
            return NotFound();

        ViewBag.Nombre = estudiante.NombreCompleto;
        ViewBag.Carrera = estudiante.Carrera?.Nombre;
        ViewBag.TotalCursos = estudiante.Matriculas?.Count ?? 0;
        ViewBag.FotoUrl = estudiante.FotoUrl;

        // Barra
        var totalCursosCarrera = await _context.Cursos
            .CountAsync(c => c.CarreraId == estudiante.CarreraId);

        var cursosMatriculados = estudiante.Matriculas?.Count ?? 0;

        int porcentaje = totalCursosCarrera == 0
            ? 0
            : (int)((double)cursosMatriculados / totalCursosCarrera * 100);

        ViewBag.Progreso = porcentaje;
        ViewBag.TotalCarrera = totalCursosCarrera;

        return View(estudiante);
    }

    // GET EDIT
    public async Task<IActionResult> Edit()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var estudiante = await _context.Users
            .Include(u => u.Carrera)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (estudiante == null)
            return NotFound();

        ViewBag.Carreras = await _context.Carreras.ToListAsync();
        ViewBag.FotoUrl = estudiante.FotoUrl;

        return View(estudiante);
    }

    // POST EDIT
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model, IFormFile foto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var estudiante = await _context.Users
            .Include(u => u.Matriculas)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (estudiante == null)
            return NotFound();

        // Cambiar Carrera
        bool cambioCarrera = estudiante.CarreraId != model.CarreraId;

        // Actuaizar
        estudiante.NombreCompleto = model.NombreCompleto;
        estudiante.Email = model.Email;
        estudiante.CarreraId = model.CarreraId;

        // Foto firebase
        if (foto != null && foto.Length > 0)
        {
            var url = await _firebase.SubirImagenAsync(foto);
            estudiante.FotoUrl = url;
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
    }
}