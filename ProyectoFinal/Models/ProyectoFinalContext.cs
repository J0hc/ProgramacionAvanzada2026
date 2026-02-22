using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal.Models
{
    public class ProyectoFinalContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor
        public ProyectoFinalContext(DbContextOptions<ProyectoFinalContext> options)
            : base(options)
        {
        }

       
        public DbSet<Carrera> Carreras { get; set; } 

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Entidades
            builder.Entity<Carrera>()
                   .ToTable("Carreras")
                   .HasKey(c => c.Id); 
        }
    }
}