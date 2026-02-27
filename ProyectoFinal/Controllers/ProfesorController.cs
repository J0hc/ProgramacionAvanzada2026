using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using System.Threading.Tasks;

[Authorize(Roles = "Administrador")]
public class ProfesorController : Controller
{
    private readonly ProyectoFinalContext _context;

    public ProfesorController(ProyectoFinalContext context)
    {
        _context = context;
    }

    // GET: Profesores
    public async Task<IActionResult> Index()
    {
        var profesores = await _context.Profesores.ToListAsync();
        return View(profesores);
    }

    // GET: Crear
    public IActionResult Create()
    {
        return View();
    }

    // POST: Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Profesor profesor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(profesor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(profesor);
    }

    // GET: Editar
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var profesor = await _context.Profesores.FindAsync(id);
        if (profesor == null) return NotFound();

        return View(profesor);
    }

    // POST: Editar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Profesor profesor)
    {
        if (id != profesor.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(profesor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(profesor);
    }

    // GET: Eliminar
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var profesor = await _context.Profesores
            .FirstOrDefaultAsync(p => p.Id == id);

        if (profesor == null) return NotFound();

        return View(profesor);
    }

    // POST: Confirmar eliminación
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var profesor = await _context.Profesores.FindAsync(id);
        _context.Profesores.Remove(profesor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}