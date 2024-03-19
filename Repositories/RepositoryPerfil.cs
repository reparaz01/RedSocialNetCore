using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Data;
using RedSocialNetCore.Models;

namespace RedSocialNetCore.Repositories
{
    public class RepositoryPerfil
    {
        private ContextApp context;

        public RepositoryPerfil(ContextApp context)
        {
            this.context = context;
        }


        public List<Publicacion> GetPublicacionesUsuario(string username)
        {
            return context.Publicaciones
                .Where(p => p.Username == username)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToList();
        }

        public Usuario GetUser(string username)
        {
            var usuario = (from u in context.Usuarios
                           where u.Username == username
                           select u).FirstOrDefault();

            return usuario;
        }

        public bool IsLiked(int idPublicacion, string username)
        {
            return context.Likes.Any(l => l.IdPublicacion == idPublicacion && l.Username == username);
        }

        public void Like(int idPublicacion, string username)
        {
            if (!IsLiked(idPublicacion, username))
            {
                Like like = new Like { IdPublicacion = idPublicacion, Username = username };
                context.Likes.Add(like);
                context.SaveChanges();
            }
        }

        public void Dislike(int idPublicacion, string username)
        {
            Like like = context.Likes.FirstOrDefault(l => l.IdPublicacion == idPublicacion && l.Username == username);
            if (like != null)
            {
                context.Likes.Remove(like);
                context.SaveChanges();
            }
        }

        public int GetLikes(int idPublicacion)
        {
            return context.Likes.Count(l => l.IdPublicacion == idPublicacion);
        }


        public int GetSeguidosCount(string username)
        {
            return context.Seguidores.Count(s => s.SeguidorUsername == username);
        }

        public int GetSeguidoresCount(string username)
        {
            return context.Seguidores.Count(s => s.SeguidoUsername == username);
        }


        public bool IsFollowing(string seguidorUsername, string seguidoUsername)
        {
            return context.Seguidores.Any(s => s.SeguidorUsername == seguidorUsername && s.SeguidoUsername == seguidoUsername);
        }


        public void Follow(string seguidorUsername, string seguidoUsername)
        {
            // Verificar si ya existe una relación de seguimiento
            if (!IsFollowing(seguidorUsername, seguidoUsername))
            {
                // Crear una nueva relación de seguimiento
                Seguidores seguimiento = new Seguidores
                {
                    SeguidorUsername = seguidorUsername,
                    SeguidoUsername = seguidoUsername
                };

                // Agregar la nueva relación de seguimiento al contexto
                context.Seguidores.Add(seguimiento);

                // Guardar los cambios en la base de datos
                context.SaveChanges();
            }
        }

        public void Unfollow(string seguidorUsername, string seguidoUsername)
        {
            // Obtener la relación de seguimiento existente, si existe
            Seguidores seguimiento = context.Seguidores.FirstOrDefault(s => s.SeguidorUsername == seguidorUsername && s.SeguidoUsername == seguidoUsername);

            // Verificar si la relación de seguimiento existe
            if (seguimiento != null)
            {
                // Eliminar la relación de seguimiento del contexto
                context.Seguidores.Remove(seguimiento);

                // Guardar los cambios en la base de datos
                context.SaveChanges();
            }
        }

        [HttpPost]
        public void EliminarPublicacion(int idPublicacion, string username)
        {
            // Obtener la publicación a eliminar
            Publicacion publicacion = context.Publicaciones.FirstOrDefault(p => p.IdPublicacion == idPublicacion && p.Username == username);

            // Verificar si se encontró la publicación
            if (publicacion != null)
            {
                // Eliminar los likes asociados a la publicación
                var likes = context.Likes.Where(l => l.IdPublicacion == idPublicacion);
                context.Likes.RemoveRange(likes);

                // Eliminar la publicación del contexto
                context.Publicaciones.Remove(publicacion);

                // Guardar los cambios en la base de datos
                context.SaveChanges();
            }
        }





    }
}
