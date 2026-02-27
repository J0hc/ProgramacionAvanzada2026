using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace CrudCursos.Controllers
{
    public class CursoController : Controller
    {
        private readonly ProyectoFinalContext _context;
        private readonly IWebHostEnvironment _env;

        public CursoController(ProyectoFinalContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Cursos
        public async Task<IActionResult> Index()
        {
            var cursos = _context.Cursos
                .Include(c => c.Carrera)
                .Include(c => c.Profesor);
            return View(await cursos.ToListAsync());
        }

        // GET: Crear
        public IActionResult Create()
        {
            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre");
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto");
            return View();
        }

        // POST: Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                if (curso.Imagen != null)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var filePath = Path.Combine(uploads, curso.Imagen.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await curso.Imagen.CopyToAsync(stream);
                    }
                    curso.ImagenUrl = "/images/" + curso.Imagen.FileName;
                }

                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto", curso.ProfesorId);
            return View(curso);
        }

        // GET: Cursos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            ViewData["Carreras"] = new SelectList(_context.Carreras, "Id", "Nombre", curso.CarreraId);
            ViewData["Profesores"] = new SelectList(_context.Profesores, "Id", "NombreCompleto", curso.ProfesorId);
            return View(curso);
        }

        // POST: Cursos/Edit/5
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

                    if (curso.Imagen != null)
                    {
                        var uploads = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(uploads))
                            Directory.CreateDirectory(uploads);

                        var filePath = Path.Combine(uploads, curso.Imagen.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await curso.Imagen.CopyToAsync(stream);
                        }
                        cursoDb.ImagenUrl = "/images/" + curso.Imagen.FileName;
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

        // GET: Cursos/Delete/5
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

        // POST: Cursos/Delete/5
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

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}