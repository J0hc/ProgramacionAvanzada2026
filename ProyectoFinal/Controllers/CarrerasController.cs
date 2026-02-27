using Microsoft.AspNetCore.Mvc;
using global::ProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Controllers
{
    public class CarrerasController : Controller
    {
        private readonly ProyectoFinalContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarrerasController(ProyectoFinalContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // PARA LISTAR
        public async Task<IActionResult> Index()
        {
            var carreras = await _context.Carreras.ToListAsync();
            return View(carreras);
        }

        // CREATE GET
        public IActionResult Create() => View();

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                if (carrera.Imagen != null)
                {
                    string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes/carreras");
                    Directory.CreateDirectory(uploads);

                    string fileName = Guid.NewGuid() + Path.GetExtension(carrera.Imagen.FileName);
                    string filePath = Path.Combine(uploads, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                        await carrera.Imagen.CopyToAsync(fs);

                    carrera.ImagenUrl = "/imagenes/carreras/" + fileName;
                }

                _context.Carreras.Add(carrera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(carrera);
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null) return NotFound();
            return View(carrera);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Carrera carrera)
        {
            if (ModelState.IsValid)
            {
                var carreraDb = await _context.Carreras.FindAsync(carrera.Id);
                if (carreraDb == null) return NotFound();

                carreraDb.Nombre = carrera.Nombre;
                carreraDb.Escuela = carrera.Escuela;

                if (carrera.Imagen != null)
                {
                    string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes/carreras");
                    Directory.CreateDirectory(uploads);

                    string fileName = Guid.NewGuid() + Path.GetExtension(carrera.Imagen.FileName);
                    string filePath = Path.Combine(uploads, fileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                        await carrera.Imagen.CopyToAsync(fs);

                    carreraDb.ImagenUrl = "/imagenes/carreras/" + fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carrera);
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                _context.Carreras.Remove(carrera);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null) return NotFound();
            return View(carrera);
        }
    }
}