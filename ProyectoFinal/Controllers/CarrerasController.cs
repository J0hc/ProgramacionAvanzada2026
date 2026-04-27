using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Controllers
{
    public class CarrerasController : Controller
    {
        private readonly ProyectoFinalContext _context;
        private readonly FirebaseStorageService _firebase;

        //FIREBASE
        public CarrerasController(ProyectoFinalContext context, FirebaseStorageService firebase)
        {
            _context = context;
            _firebase = firebase;
        }

        // INDEX
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
                // Subir Firebase
                if (carrera.Imagen != null && carrera.Imagen.Length > 0)
                {
                    var url = await _firebase.SubirImagenAsync(carrera.Imagen);
                    carrera.ImagenUrl = url;
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

               if (carrera.Imagen != null && carrera.Imagen.Length > 0)
                {
                    var url = await _firebase.SubirImagenAsync(carrera.Imagen);
                    carreraDb.ImagenUrl = url;
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