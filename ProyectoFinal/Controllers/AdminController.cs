using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers;


[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
}