using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

public static class UserHelper
{
    public static async Task<ApplicationUser> GetUsuarioAsync(
        ProyectoFinalContext context,
        string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return null;

        return await context.Users
            .Include(u => u.Carrera)
            .Include(u => u.Matriculas)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}