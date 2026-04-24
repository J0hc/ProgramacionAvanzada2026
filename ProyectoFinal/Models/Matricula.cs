namespace ProyectoFinal.Models;
public class Matricula
{
    public int Id { get; set; }

    public string EstudianteId { get; set; }
    public ApplicationUser Estudiante { get; set; }

    public int CursoId { get; set; }
    public Curso Curso { get; set; }
}