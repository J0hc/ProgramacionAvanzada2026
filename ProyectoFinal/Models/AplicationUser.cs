using Microsoft.AspNetCore.Identity;
namespace ProyectoFinal.Models;


public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; }
}