using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Models
{
    public class ProyectoFinalContext : IdentityDbContext<ApplicationUser>
    {
        public ProyectoFinalContext(DbContextOptions<ProyectoFinalContext> options)
            : base(options)
        {
        }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Curso> Cursos { get; set; }

        public DbSet<Matricula> Matriculas { get; set; }

        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tabla Carreras
            builder.Entity<Carrera>()
                   .ToTable("Carreras")
                   .HasKey(c => c.Id);

            // User - Carrera
            builder.Entity<ApplicationUser>()
                   .HasOne(u => u.Carrera)
                   .WithMany(c => c.Estudiantes)
                   .HasForeignKey(u => u.CarreraId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}