using System;

namespace ProyectoFinal.Models
{
    public class Auditoria
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public string NombreUsuario { get; set; } // 🔥 NUEVO

        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}