using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal.Models

{
    public class Profesor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido")]
        public string Email { get; set; }

        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }
    }
}