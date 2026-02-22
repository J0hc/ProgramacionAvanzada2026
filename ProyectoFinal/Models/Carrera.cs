using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProyectoFinal.Models
{
    public class Carrera
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La escuela es obligatoria")]
        public string Escuela { get; set; }

        //[ValidateNever]
        //public List<Curso> Cursos { get; set; }

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? Imagen { get; set; }
    }
}