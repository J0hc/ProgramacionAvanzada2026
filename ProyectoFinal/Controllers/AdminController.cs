using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Controllers;

[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    private readonly ProyectoFinalContext _context;

    public AdminController(ProyectoFinalContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        // Variables totales del sistema
        ViewBag.TotalCarreras = _context.Carreras.Count();
        ViewBag.TotalCursos = _context.Cursos.Count();
        ViewBag.TotalProfesores = _context.Profesores.Count();

        return View();
    }

    //Para manejar vista de auditoria
    public async Task<IActionResult> Auditoria()
    {
        var logs = await _context.Auditorias
            .OrderByDescending(a => a.Fecha)
            .ToListAsync();

        return View(logs);
    }
}