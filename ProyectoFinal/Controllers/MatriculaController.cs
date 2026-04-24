using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using System.Security.Claims;

namespace ProyectoFinal.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly ProyectoFinalContext _context;

        public MatriculaController(ProyectoFinalContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var estudiante = await _context.Users
                .Include(u => u.Carrera)
                .Include(u => u.Matriculas)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (estudiante == null || estudiante.CarreraId == null)
                return Content("Error: El estudiante no tiene carrera asignada.");

            // Pasar Data 
            ViewBag.Nombre = estudiante.NombreCompleto;
            ViewBag.Carrera = estudiante.Carrera?.Nombre;
            ViewBag.TotalCursos = estudiante.Matriculas?.Count ?? 0;
            ViewBag.FotoUrl = estudiante?.FotoUrl;

            var cursosQuery = _context.Cursos
                .Include(c => c.Profesor)
                .Where(c => c.CarreraId == estudiante.CarreraId);

            if (!string.IsNullOrEmpty(search))
            {
                cursosQuery = cursosQuery
                    .Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
            }

            var cursos = await cursosQuery.ToListAsync();

            var cursosMatriculados = estudiante.Matriculas
                .Select(m => m.CursoId)
                .ToList();

            ViewBag.CursosMatriculados = cursosMatriculados;
            ViewBag.TotalMatriculados = cursosMatriculados.Count;
            ViewBag.Search = search;

            return View("Create", cursos);
        }

        // POST MATRICULAR
        [HttpPost]
        public async Task<IActionResult> Create(int cursoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var total = await _context.Matriculas
                .CountAsync(m => m.EstudianteId == userId);

            if (total >= 4)
                return RedirectToAction("Index");

            var existe = await _context.Matriculas
                .AnyAsync(m => m.EstudianteId == userId && m.CursoId == cursoId);

            if (!existe)
            {
                var matricula = new Matricula
                {
                    EstudianteId = userId,
                    CursoId = cursoId
                };

                _context.Matriculas.Add(matricula);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // BORRAR
        [HttpPost]
        public async Task<IActionResult> Delete(int cursoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.EstudianteId == userId && m.CursoId == cursoId);

            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}