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
        var usuario = await _context.Users
            .Include(u => u.Carrera)
            .FirstOrDefaultAsync(u => u.Id == userId);

        // Pasar
        return View(usuario);
    }
}