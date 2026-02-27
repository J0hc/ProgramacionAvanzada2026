using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Registro
{
    [Required]
    public string NombreCompleto { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Required, DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; }


    [Display(Name = "Carrera")]
    [Required(ErrorMessage = "Selecciona una carrera")]
    public int CarreraId { get; set; }

    // Dropdown
    [ValidateNever]
    public IEnumerable<SelectListItem> Carreras { get; set; }
}