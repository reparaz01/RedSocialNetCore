using RedSocialNetCore.Data;
using RedSocialNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace RedSocialNetCore.Repositories
{
    public class RepositoryPublicacion
    {
        private ContextApp context;

        public RepositoryPublicacion(ContextApp context)
        {
            this.context = context;
        }

        public async Task AddPublicacion(Publicacion publicacion)
        {
            // Verificar si alguno de los campos de la publicación es nulo
            if (publicacion == null || publicacion.Texto == null || publicacion.FechaPublicacion == null || publicacion.Username == null)
            {
                throw new ArgumentNullException("Alguno de los campos de la publicación es nulo.");
            }

            // Agregar la nueva publicación al contexto de la base de datos
            context.Publicaciones.Add(publicacion);

            // Guardar los cambios en la base de datos
            await context.SaveChangesAsync();
        }
        public int GetNextPublicacionId()
        {
            // Consultar el máximo ID de la tabla de publicaciones
            int maxId = context.Publicaciones.Max(p => (int?)p.IdPublicacion) ?? 0;

            // Incrementar el máximo ID para obtener el siguiente ID disponible
            return maxId + 1;
        }


    }
}
