// Repositorio
using System.Collections.Generic;
using System.Linq;
using RedSocialNetCore.Data;
using RedSocialNetCore.Models;

public class RepositoryHome
{
    private readonly ContextApp context;

    public RepositoryHome(ContextApp context)
    {
        this.context = context;
    }

    public List<Publicacion> GetAllPublicacionesExceptoUsuario(string username)
    {
        return context.Publicaciones
            .Where(p => p.Username != username)
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

    public List<Publicacion> GetAllPublicacionesSeguidosExceptoUsuario(string username)
    {
        // Obtener los nombres de usuario de los seguidos por el usuario actual
        var seguidos = context.Seguidores
            .Where(s => s.SeguidorUsername == username)
            .Select(s => s.SeguidoUsername)
            .ToList();

        // Obtener las publicaciones de los usuarios seguidos, excluyendo al usuario actual
        var publicacionesSeguidos = context.Publicaciones
            .Where(p => seguidos.Contains(p.Username) && p.Username != username)
            .OrderByDescending(p => p.FechaPublicacion)
            .ToList();

        return publicacionesSeguidos;
    }


    public int GetLikes(int idPublicacion)
    {
        return context.Likes.Count(l => l.IdPublicacion == idPublicacion);
    }







}
