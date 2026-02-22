using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}