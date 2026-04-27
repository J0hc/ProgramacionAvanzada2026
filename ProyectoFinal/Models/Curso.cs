using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los creditos son obligatorios")]
        public int Creditos { get; set; }

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? Imagen { get; set; }

        [Display(Name = "Carrera")]
        public int CarreraId { get; set; }
        [ValidateNever]
        public Carrera Carrera { get; set; }

        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }
        [ValidateNever]
        public Profesor Profesor { get; set; }
    }
}