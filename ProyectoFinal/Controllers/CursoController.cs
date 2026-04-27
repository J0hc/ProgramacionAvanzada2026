using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudCursos.Controllers
{

    public class CursoController : Controller
    {
        private readonly ProyectoFinalContext _context;
        private readonly FirebaseStorageService _firebase;
        

        //FIREBASE
        public CursoController(ProyectoFinalContext context, FirebaseStorageService firebase)
        {
            _context = context;
            _firebase = firebase;
            
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var cursos = _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Profesor);

            return View(await cursos.ToListAsync());
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto");
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                if (curso.Imagen != null && curso.Imagen.Length > 0)
                {
                    var url = await _firebase.SubirImagenAsync(curso.Imagen);
                    curso.ImagenUrl = url;
                }

                _context.Add(curso);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto", curso.ProfesorId);

            return View(curso);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto", curso.ProfesorId);

            return View(curso);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Curso curso)
        {
            if (id != curso.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var cursoDb = await _context.Cursos.FindAsync(id);
                    if (cursoDb == null) return NotFound();

                    cursoDb.Nombre = curso.Nombre;
                    cursoDb.Creditos = curso.Creditos;
                    cursoDb.CarreraId = curso.CarreraId;
                    cursoDb.ProfesorId = curso.ProfesorId;

                    if (curso.Imagen != null && curso.Imagen.Length > 0)
                    {
                        var url = await _firebase.SubirImagenAsync(curso.Imagen);
                        cursoDb.ImagenUrl = url;
                    }

                    _context.Update(cursoDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto", curso.ProfesorId);

            return View(curso);
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null) return NotFound();

            return View(curso);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // VALIDACIÓN
        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}