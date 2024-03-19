using Microsoft.EntityFrameworkCore;
using RedSocialNetCore.Data;
using RedSocialNetCore.Models;

namespace RedSocialNetCore.Repositories
{
    public class RepositoryEditarPerfil
    {

        private ContextApp context;

        public RepositoryEditarPerfil(ContextApp context)
        {
            this.context = context;
        }

        public Usuario GetUser(string username)
        {
            var usuario = (from u in context.Usuarios
                           where u.Username == username
                           select u).FirstOrDefault();

            return usuario;
        }

        public async Task UpdateUser(Usuario usuario)
        {
            var user = context.Usuarios.FirstOrDefault(u => u.Username == usuario.Username);

            if (user != null)
            {
                // Verificar y asignar los valores, utilizando cadenas vacías en lugar de null si es necesario
                user.Nombre = string.IsNullOrEmpty(usuario.Nombre) ? string.Empty : usuario.Nombre;
                user.Email = string.IsNullOrEmpty(usuario.Email) ? string.Empty : usuario.Email;
                user.Telefono = string.IsNullOrEmpty(usuario.Telefono) ? string.Empty : usuario.Telefono;
                user.Descripcion = string.IsNullOrEmpty(usuario.Descripcion) ? string.Empty : usuario.Descripcion;

                // Si se proporciona una nueva foto de perfil, actualizarla
                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    user.FotoPerfil = usuario.FotoPerfil;

                    var sql = $"UPDATE Publicaciones SET foto_perfil = '{usuario.FotoPerfil}' WHERE username = '{usuario.Username}'";

                    // Ejecuta el comando SQL
                    await context.Database.ExecuteSqlRawAsync(sql);

                }

                // Marcar la entidad como modificada para que se actualice en la base de datos
                context.Entry(user).State = EntityState.Modified;

                // Guardar los cambios en la base de datos
                await context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Usuario no encontrado");
            }
        }




    }
}
