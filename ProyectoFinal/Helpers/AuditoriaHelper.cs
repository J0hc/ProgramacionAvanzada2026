using ProyectoFinal.Models;

public static class AuditoriaHelper
{
    public static async Task Registrar(
        ProyectoFinalContext context,
        string usuarioId,
        string nombreUsuario,
        string accion,
        string entidad,
        string descripcion)
    {
        var log = new Auditoria
        {
            UsuarioId = usuarioId,
            NombreUsuario = nombreUsuario,
            Accion = accion,
            Entidad = entidad,
            Descripcion = descripcion,
            Fecha = DateTime.Now
        };

        context.Auditorias.Add(log);
        await context.SaveChangesAsync();
    }
}