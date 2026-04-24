using Microsoft.AspNetCore.Identity;
namespace ProyectoFinal.Models;


public class ApplicationUser : IdentityUser
{
    public string NombreCompleto { get; set; }

    // Asociar Carreras
    public int? CarreraId { get; set; } 
    public virtual Carrera Carrera { get; set; }

    public List<Matricula> Matriculas { get; set; }

    public string? FotoUrl { get; set; }
}